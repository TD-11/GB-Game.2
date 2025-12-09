using UnityEngine.UI;
using System;
using System.Collections;
using System.IO.Ports;
using System.Threading;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SD_Serial : MonoBehaviour
{
    [Header("UI")]
    public TMP_Dropdown portDropdown;
    public Button connectButton;
    public GameObject SetConnectButton;
    public TMP_Text statusText;
    public TMP_Text valuesText;

    [Header("Mostrador de portas")]
    public TMP_Text portListText; 

    [Header("Configurações")]
    public int baudRate = 57600;
    public float pollInterval = 1f;
    public float connectTimeout = 15f;

    [Header("Cronômetro")]
    public TMP_Text timerText;         // <-- NOVO
    public CanvasGroup uiBlocker;      // <-- NOVO

    private SerialPort serialPort;
    private Thread readThread;
    private bool _reading = false;
    public static bool _connected = false;
    private bool _waitingConnection = false;

    private string buffer = "";
    
    public static string selectedPort;

    // Valores recebidos
    public float A, B, C, D, P = 0f;

    private float nextPollTime = 0f;

    public string State;

    void Start()
    {
        UpdatePortList();
        DontDestroyOnLoad(this.gameObject);

        connectButton.onClick.AddListener(OnConnectButtonPressed);

        statusText.text = "Selecione uma porta";

        // Desativa UIBlocker no início
        if (uiBlocker != null)
        {
            uiBlocker.blocksRaycasts = false;
            uiBlocker.interactable = false;
        }

        if (timerText != null)
            timerText.text = "";
    }

    void Update()
    {
        if (_connected) return;
        if (_waitingConnection) return;

        if (Time.time > nextPollTime)
        {
            UpdatePortList();
            nextPollTime = Time.time + pollInterval;
        }
    }


    void UpdatePortList()
    {
        string[] ports = SerialPort.GetPortNames();

        List<string> portList = new List<string>();
        foreach (string port in ports)
        {
            if (!port.Equals("COM1", StringComparison.OrdinalIgnoreCase))
            {
                portList.Add(port);
            }
        }

        if (portListText != null)
        {
            if (portList.Count > 0)
                portListText.text = "Portas encontradas:\n" + string.Join("\n", portList);
            else
                portListText.text = "Nenhuma porta encontrada";
        }

        if (DropdownListChanged(portList))
        {
            string currentSelectedPort = null;
            if (portDropdown.options.Count > 0 &&
                portDropdown.value >= 0 &&
                portDropdown.value < portDropdown.options.Count)
            {
                currentSelectedPort = portDropdown.options[portDropdown.value].text;
            }

            portDropdown.ClearOptions();
            portDropdown.AddOptions(portList);

            if (!string.IsNullOrEmpty(currentSelectedPort))
            {
                int restoredIndex = portList.IndexOf(currentSelectedPort);
                if (restoredIndex >= 0)
                {
                    portDropdown.value = restoredIndex;
                }
            }

            if (portList.Count == 0)
                statusText.text = "Nenhuma porta encontrada";
        }
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

    public void OnConnectButtonPressed()
    {
        Debug.Log("OnConnectButtonPressed");

        // --- INICIA O CRONÔMETRO AO CLICAR ---
        StartCoroutine(StartCountdown());

        string selectedPort = portDropdown.options[portDropdown.value].text;
        Debug.Log($"Trying to connect to {selectedPort}");

        if (!_connected)
        {
            SetConnectButton.SetActive(false);
            StartCoroutine(ConnectWithTimeout());   
        }
        else
        {
            Disconnect();
        }
    }


    IEnumerator ConnectWithTimeout()
    {
        Debug.Log("ConnectWithTimeout");

        if (_waitingConnection)
            yield break;

        if (portDropdown.options.Count == 0)
        {
            statusText.text = "Nenhuma porta disponível";
            yield break;
        }
        
        string selectedPort = portDropdown.options[portDropdown.value].text;

        Debug.Log($"Conectando a {selectedPort}...");
        
        statusText.text = $"Conectando a {selectedPort}...";
        _waitingConnection = true;

        bool connected = false;

        Thread connectThread = new Thread(() =>
        {
            try
            {
                serialPort = new SerialPort(selectedPort, baudRate);
                serialPort.ReadTimeout = 100;
                serialPort.Open();
                connected = true;
            }
            catch
            {
                connected = false;
            }
        });

        connectThread.Start();

        float startTime = Time.time;

        while (Time.time - startTime < connectTimeout)
        {
            if (connected) break;
            yield return null;
        }

        _waitingConnection = false;

        if (!connected)
        {
            statusText.text = "Erro: conexão expirou (15s)";
            SetConnectButton.SetActive(true);        

            try { serialPort?.Close(); } catch { }
            yield break;
        }

        _connected = true;
        
        statusText.text = "Porta conectada";
        SetConnectButton.SetActive(false);

        StartReading();
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
        try{
            
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
        
        valuesText.text =
            $"A: {A}\n" +
            $"B: {B}\n" +
            $"C: {C}\n" +
            $"D: {D}\n" +
            $"P: {P}";
            
        Debug.Log(valuesText.text);

     }
    catch (Exception e)
    {
        Exception rootCause = e.GetBaseException();
        if (rootCause.GetType() == typeof(TimeoutException) )
        {
            State = "Data Loading: Timeout";
            Debug.Log(State);
        }
        else
        {
            State = "Data Received Erro: " + e.Message;
            Debug.Log(State);
        }
    }
    
}

    void Disconnect()
    {
        _reading = false;
        _connected = false;

        try { readThread?.Abort(); } catch { }

        if (serialPort != null && serialPort.IsOpen)
            serialPort.Close();

        statusText.text = "Desconectado";
    }

    void OnApplicationQuit()
    {
        Disconnect();
    }


    // -----------------------------
    //        CRONÔMETRO
    // -----------------------------
    public IEnumerator StartCountdown()
    {
        // Ativa bloqueio de interface
        uiBlocker.blocksRaycasts = true;
        uiBlocker.interactable = false;

        float timeLeft = 15f;

        while (timeLeft > 0)
        {
            timerText.text = "Aguarde: " + Mathf.Ceil(timeLeft).ToString();
            timeLeft -= Time.deltaTime;
            yield return null;
        }

        timerText.text = "";

        // Libera interface
        uiBlocker.blocksRaycasts = false;
        uiBlocker.interactable = true;
        
        SetConnectButton.SetActive(true);  // <- REAPARECE O BOTÃO

    }
}