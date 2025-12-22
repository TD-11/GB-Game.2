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
    // =========================
    //          UI
    // =========================
    [Header("UI")]
    public TMP_Dropdown portDropdown;      // Dropdown com as portas COM disponíveis
    public Button connectButton;           // Botão de conectar
    public GameObject SetConnectButton;    // Objeto do botão (ativar/desativar)
    public TMP_Text statusText;             // Texto de status da conexão
    public TMP_Text valuesText;             // Texto com os valores recebidos

    // =========================
    //     Mostrador de portas
    // =========================
    [Header("Mostrador de portas")]
    public TMP_Text portListText;           // Lista textual das portas detectadas

    // =========================
    //       Configurações
    // =========================
    [Header("Configurações")]
    public int baudRate = 57600;             // Velocidade da comunicação serial
    public float pollInterval = 1f;          // Intervalo de atualização da lista de portas
    public float connectTimeout = 15f;       // Tempo máximo de espera para conexão

    // =========================
    //         Cronômetro
    // =========================
    [Header("Cronômetro")]
    public TMP_Text timerText;               // Texto do contador regressivo
    public CanvasGroup uiBlocker;             // Bloqueia a interface durante a conexão

    // =========================
    //    Controle da Serial
    // =========================
    private SerialPort serialPort;            // Porta serial
    private Thread readThread;                // Thread de leitura
    private bool _reading = false;             // Indica se está lendo dados
    public static bool _connected = false;    // Estado global da conexão
    private bool _waitingConnection = false;  // Evita múltiplas tentativas simultâneas

    private string buffer = "";               // Buffer para leitura de mensagens

    public static string selectedPort;        // Porta selecionada

    // =========================
    //     Dados recebidos
    // =========================
    public float A, B, C, D, P = 0F;           // Sensores individuais
    public static float S = 0F;                // Soma dos sensores
    private float nextPollTime = 0f;           // Controle do tempo de atualização

    public string State;                       // Estado atual (debug)

    void Start()
    {
        // Atualiza a lista de portas ao iniciar
        UpdatePortList();

        // Associa o botão ao método de conexão
        connectButton.onClick.AddListener(OnConnectButtonPressed);

        // Texto inicial
        statusText.text = "Selecione uma porta";

        // Desbloqueia a interface
        if (uiBlocker != null)
        {
            uiBlocker.blocksRaycasts = false;
            uiBlocker.interactable = false;
        }

        // Limpa o cronômetro visual
        if (timerText != null)
            timerText.text = "";
    }

    void Update()
    {
        // Se já estiver conectado ou aguardando conexão, não faz nada
        if (_connected) return;
        if (_waitingConnection) return;

        // Atualiza a lista de portas periodicamente
        if (Time.time > nextPollTime)
        {
            UpdatePortList();
            nextPollTime = Time.time + pollInterval;
        }
    }

    // =========================
    // Atualiza lista de portas
    // =========================
    void UpdatePortList()
    {
        string[] ports = SerialPort.GetPortNames();

        List<string> portList = new List<string>();

        // Remove a COM1 (geralmente porta interna da placa-mãe)
        foreach (string port in ports)
        {
            if (!port.Equals("COM1", StringComparison.OrdinalIgnoreCase))
            {
                portList.Add(port);
            }
        }

        // Mostra as portas detectadas na UI
        if (portListText != null)
        {
            if (portList.Count > 0)
                portListText.text = "Portas encontradas:\n" + string.Join("\n", portList);
            else
                portListText.text = "Nenhuma porta encontrada";
        }

        // Atualiza o dropdown apenas se houver mudança
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

            // Restaura a seleção anterior se possível
            if (!string.IsNullOrEmpty(currentSelectedPort))
            {
                int restoredIndex = portList.IndexOf(currentSelectedPort);
                if (restoredIndex >= 0)
                {
                    portDropdown.value = restoredIndex;
                }
            }

            if (portList.Count == 0)
                statusText.text = "Selecione uma porta";
        }
    }

    // Verifica se a lista do dropdown mudou
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

    // =========================
    // Botão Conectar
    // =========================
    public void OnConnectButtonPressed()
    {
        Debug.Log("OnConnectButtonPressed");

        // Texto durante o tempo de espera
        statusText.text = "Conectando...";

        // Inicia o cronômetro visual
        StartCoroutine(StartCountdown());

        string selectedPort = portDropdown.options[portDropdown.value].text;
        Debug.Log($"Trying to connect to {selectedPort}");

        if (!_connected)
        {
            // Desativa o botão durante a tentativa
            SetConnectButton.SetActive(false);
            StartCoroutine(ConnectWithTimeout());
        }
        else
        {
            Disconnect();
        }
    }

    // =========================
    // Conexão com timeout
    // =========================
    IEnumerator ConnectWithTimeout()
    {
        if (_waitingConnection)
            yield break;

        if (portDropdown.options.Count == 0)
        {
            statusText.text = "Selecione uma porta";
            yield break;
        }

        string selectedPort = portDropdown.options[portDropdown.value].text;
        _waitingConnection = true;

        bool connected = false;

        // Thread para não travar a UI
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

        // Aguarda até conectar ou estourar o tempo
        while (Time.time - startTime < connectTimeout)
        {
            if (connected) break;
            yield return null;
        }

        _waitingConnection = false;

        if (!connected)
        {
            SetConnectButton.SetActive(true);
            try { serialPort?.Close(); } catch { }
            yield break;
        }

        _connected = true;
        SetConnectButton.SetActive(false);

        // Inicia leitura contínua
        StartReading();
    }

    // =========================
    // Leitura dos dados
    // =========================
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

                // Fim da mensagem
                if (c == '\n')
                {
                    ProcessMessage(buffer);
                    buffer = "";
                }
            }
            catch { }
        }
    }

    // Processa a mensagem recebida
    void ProcessMessage(string msg)
    {
        try
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

            // Soma total dos sensores
            S = A + B + C + D;

            // Atualiza UI
            valuesText.text =
                $"A: {A}\n" +
                $"B: {B}\n" +
                $"C: {C}\n" +
                $"D: {D}\n" +
                $"P: {P}\n" +
                $"S: {S}\n";
        }
        catch (Exception e)
        {
            Exception rootCause = e.GetBaseException();
            State = rootCause is TimeoutException
                ? "Data Loading: Timeout"
                : "Data Received Erro: " + e.Message;

            Debug.Log(State);
        }
    }

    // =========================
    // Desconexão
    // =========================
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

    // =========================
    //        CRONÔMETRO
    // =========================
    public IEnumerator StartCountdown()
    {
        // Bloqueia interação
        uiBlocker.blocksRaycasts = true;
        uiBlocker.interactable = false;

        float timeLeft = 15f;

        // Contagem regressiva visual
        while (timeLeft > 0)
        {
            timerText.text = "Aguarde: " + Mathf.Ceil(timeLeft);
            timeLeft -= Time.deltaTime;
            yield return null;
        }

        timerText.text = "";

        // Após o tempo, volta ao estado inicial
        statusText.text = "Selecione uma porta";

        uiBlocker.blocksRaycasts = false;
        uiBlocker.interactable = true;

        SetConnectButton.SetActive(true);
    }
}