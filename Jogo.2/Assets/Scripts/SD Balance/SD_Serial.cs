using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO.Ports;
using System.Threading;
using System.Collections.Generic;
using TMPro;

public class SD_Serial : MonoBehaviour
{
    public static SD_Serial Instance;

    [Header("UI")]
    public TMP_Dropdown portDropdown;
    public Button connectButton;
    public TMP_Text statusText;
    public TMP_Text valuesText;
    public TMP_Text portsText;   // <- NOVO TEXTO PARA LISTAR PORTAS

    [Header("Configurações")]
    public int baudRate = 57600;
    public float pollInterval = 1f;

    private SerialPort serialPort;
    private Thread readThread;
    private bool _reading = false;
    private bool _connected = false;
    private string buffer = "";

    public float A, B, C, D, P;

    private float nextPollTime = 0f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        UpdatePortList();
        connectButton.onClick.AddListener(OnConnectButtonPressed);
        statusText.text = "Selecione uma porta";
    }

    void Update()
    {
        if (Time.time > nextPollTime)
        {
            UpdatePortList();
            nextPollTime = Time.time + pollInterval;
        }
    }

    // Atualiza lista de portas no Dropdown e no Text
    void UpdatePortList()
    {
        string[] ports = SerialPort.GetPortNames();
        List<string> portList = new List<string>(ports);

        // Atualiza Dropdown somente se houver mudança
        if (DropdownListChanged(portList))
        {
            portDropdown.ClearOptions();
            portDropdown.AddOptions(portList);
        }

        // Atualiza texto com lista de portas
        portsText.text = "Portas detectadas:\n";
        foreach (var p in portList)
            portsText.text += p + "\n";

        if (portList.Count == 0)
            statusText.text = "Nenhuma porta encontrada";
    }

    bool DropdownListChanged(List<string> newList)
    {
        if (portDropdown.options.Count != newList.Count)
            return true;

        for (int i = 0; i < newList.Count; i++)
        {
            if (portDropdown.options[i].text != newList[i])
                return true;
        }

        return false;
    }

    void OnConnectButtonPressed()
    {
        if (!_connected)
            Connect();
        else
            Disconnect();
    }

    // Conectar com timeout de 10 segundos
    void Connect()
    {
        if (portDropdown.options.Count == 0)
        {
            statusText.text = "Nenhuma porta disponível";
            return;
        }

        string selectedPort = portDropdown.options[portDropdown.value].text;

        try
        {
            serialPort = new SerialPort(selectedPort, baudRate);
            serialPort.ReadTimeout = 100;

            statusText.text = "Conectando...";

            bool success = false;
            float startTime = Time.time;

            // Tenta abrir porta por até 10 segundos
            while (Time.time - startTime < 10f)
            {
                try
                {
                    serialPort.Open();
                    success = true;
                    break;
                }
                catch
                {
                    Thread.Sleep(200); // espera antes de tentar novamente
                }
            }

            if (!success)
            {
                statusText.text = "Falha ao conectar (timeout)";
                return;
            }

            _connected = true;
            statusText.text = "Conectado em " + selectedPort;
            connectButton.GetComponentInChildren<TMP_Text>().text = "Desconectar";

            StartReading();
        }
        catch (Exception e)
        {
            statusText.text = "Erro: " + e.Message;
        }
    }

    void StartReading()
    {
        _reading = true;
        readThread = new Thread(ReadLoop);
        readThread.Start();
    }

    void ReadLoop()
    {
        while (_reading)
        {
            try
            {
                char c = (char)serialPort.ReadByte();
                buffer += c;

                if (c == '\n')
                {
                    ProcessMessage(buffer);
                    buffer = "";
                }
            }
            catch { }
        }
    }

    void ProcessMessage(string msg)
    {
        if (!msg.StartsWith("*"))
            return;

        msg = msg.Substring(1);

        string[] p = msg.Split(';');
        if (p.Length < 5) return;

        A = float.Parse(p[0].Replace('.', ','));
        B = float.Parse(p[1].Replace('.', ','));
        C = float.Parse(p[2].Replace('.', ','));
        D = float.Parse(p[3].Replace('.', ','));
        P = float.Parse(p[4].Replace('.', ','));

        UnityMainThreadDispatcher.Enqueue(() =>
        {
            valuesText.text =
                $"A: {A}\n" +
                $"B: {B}\n" +
                $"C: {C}\n" +
                $"D: {D}\n" +
                $"P: {P}";
        });
    }

    void Disconnect()
    {
        _reading = false;
        _connected = false;

        try { readThread?.Abort(); } catch { }

        if (serialPort != null && serialPort.IsOpen)
            serialPort.Close();

        connectButton.GetComponentInChildren<TMP_Text>().text = "Conectar";
        statusText.text = "Desconectado";
    }

    void OnApplicationQuit()
    {
        Disconnect();
    }
}