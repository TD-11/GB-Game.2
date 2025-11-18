using UnityEngine.UI;
using System;
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
    public TMP_Text statusText;
    public TMP_Text valuesText;

    [Header("Mostrador de portas")]
    public TMP_Text portListText; // <-- novo campo para exibir portas

    [Header("Configurações")]
    public int baudRate = 57600;
    public float pollInterval = 1f;
    public float connectTimeout = 10f; // <-- timeout de 10 segundos

    private SerialPort serialPort;
    private Thread readThread;
    private bool _reading = false;
    private bool _connected = false;
    private bool _waitingConnection = false;

    private string buffer = "";
    
    public string selectedPort;

    // Valores recebidos
    public float A, B, C, D, P;

    private float nextPollTime = 0f;

    public string State;
    
    void Start()
    {
        
        UpdatePortList();
        
        DontDestroyOnLoad(this.gameObject);
        
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

    // Atualiza lista de portas no Dropdown e no TMP_Text
    // Atualiza lista de portas no Dropdown e no TMP_Text
    void UpdatePortList()
    {
        string[] ports = SerialPort.GetPortNames();
        List<string> portList = new List<string>(ports);

        // Atualizando o texto com a lista de portas
        if (portListText != null)
        {
            if (ports.Length > 0)
                portListText.text = "Portas encontradas:\n" + string.Join("\n", ports);
            else
                portListText.text = "Nenhuma porta encontrada";
        }

        if (DropdownListChanged(portList))
        {
            // Preserve the currently selected port name before clearing
            string currentSelectedPort = null;
            if (portDropdown.options.Count > 0 && portDropdown.value >= 0 && portDropdown.value < portDropdown.options.Count)
            {
                currentSelectedPort = portDropdown.options[portDropdown.value].text;
            }

            portDropdown.ClearOptions();
            portDropdown.AddOptions(portList);

            // Restore the selection if the port is still available
            if (!string.IsNullOrEmpty(currentSelectedPort))
            {
                int restoredIndex = portList.IndexOf(currentSelectedPort);
                if (restoredIndex >= 0)
                {
                    portDropdown.value = restoredIndex;
                }
                // If not found, value remains 0 (first option)
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
        string selectedPort = portDropdown.options[portDropdown.value].text;
        Debug.Log($"Trying to connect to {selectedPort}");
        if (!_connected)
            StartCoroutine(ConnectWithTimeout());
        else
            Disconnect();
    }

    // Tentativa de conexão com timeout de 10s
    System.Collections.IEnumerator ConnectWithTimeout()
    {
        Debug.Log("ConnectWithTimeout");
        
        
        if (_waitingConnection)
            yield break;

        if (portDropdown.options.Count == 0)
        {
            Debug.Log("Nenhuma porta disponível");
            
            statusText.text = "Nenhuma porta disponível";
            yield break;
        }


        string selectedPort =  portDropdown.options[portDropdown.value].text;

        Debug.Log($"Conectando a {selectedPort}...");
        
        statusText.text = $"Conectando a {selectedPort}...";
        _waitingConnection = true;

        bool connected = false;

        // Thread de conexão para não travar o Unity
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

        // Espera até conectar ou até o timeout
        while (Time.time - startTime < connectTimeout)
        {
            if (connected) break;
            yield return null;
        }

        _waitingConnection = false;

        if (!connected)
        {
            Debug.Log("Erro: conexão expirou (10s)"); 
            
            statusText.text = "Erro: conexão expirou (10s)";
            try { serialPort?.Close(); } catch { }
            yield break;
        }

        // Se conectou
        _connected = true;
        
        Debug.Log("Conectado em " + selectedPort);
        
        statusText.text = "Conectado em " + selectedPort;
        connectButton.GetComponentInChildren<TMP_Text>().text = "Desconectar";

        StartReading();
    }

    // Iniciar thread de leitura
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

        connectButton.GetComponentInChildren<TMP_Text>().text = "Conectar";
        statusText.text = "Desconectado";
    }

    void OnApplicationQuit()
    {
        Disconnect();
    }
}