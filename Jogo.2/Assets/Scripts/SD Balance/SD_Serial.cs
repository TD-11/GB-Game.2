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
    public TMP_Text statusText;
    public TMP_Text valuesText;

    [Header("Mostrador de portas")]
    public TMP_Text portListText;

    [Header("Contagem / UI Blocker")]
    public TMP_Text timerText;
    public CanvasGroup uiBlocker;
    public GameObject SetConnectButton;

    private SerialPort serialPort;
    private Thread readThread;

    public static bool _connected = false;
    private bool _reading = false;
    private string selectedPort = "";
    private string buffer = "";

    private float nextPollTime = 0f;
    private const float pollInterval = 1f; // checa portas a cada 1s

    void Start()
    {
        UpdatePortList();
        statusText.text = "Selecione a porta";
    }

    void Update()
    {
        // Atualiza lista sempre — mesmo conectado
        if (Time.time > nextPollTime)
        {
            UpdatePortList();

            // Se conectado, mas a porta sumiu do PC → desconectar
            if (_connected && !PortStillExists(selectedPort))
            {
                Debug.Log("A porta USB foi removida!");
                statusText.text = "Conexão perdida!";
                Disconnect();
                SetConnectButton.SetActive(true);
            }

            nextPollTime = Time.time + pollInterval;
        }
    }

    // Verifica se a porta ainda existe
    bool PortStillExists(string port)
    {
        foreach (string p in SerialPort.GetPortNames())
            if (p == port)
                return true;

        return false;
    }

    public void OnConnectButtonPressed()
    {
        Debug.Log("OnConnectButtonPressed");

        StartCoroutine(StartCountdown());

        selectedPort = portDropdown.options[portDropdown.value].text;

        if (!_connected)
            StartCoroutine(ConnectWithTimeout());
        else
            Disconnect();
    }

    IEnumerator StartCountdown()
    {
        uiBlocker.blocksRaycasts = true;
        uiBlocker.interactable = false;

        float timeLeft = 15f;
        timerText.text = "Conectando...";

        while (timeLeft > 0)
        {
            timerText.text = "Conectando (" + Mathf.Ceil(timeLeft) + ")";
            timeLeft -= Time.deltaTime;
            yield return null;
        }

        timerText.text = "";

        uiBlocker.blocksRaycasts = false;
        uiBlocker.interactable = true;

        SetConnectButton.SetActive(true);
    }

    IEnumerator ConnectWithTimeout()
    {
        statusText.text = "Conectando...";
        SetConnectButton.SetActive(false);

        yield return new WaitForSeconds(0.2f);

        TryConnect();
    }

    void TryConnect()
    {
        try
        {
            serialPort = new SerialPort(selectedPort, 9600);
            serialPort.ReadTimeout = 200;

            serialPort.Open();

            _connected = true;
            _reading = true;

            statusText.text = "Conectado!";
            Debug.Log("Conectado em " + selectedPort);

            readThread = new Thread(ReadLoop);
            readThread.Start();
        }
        catch (Exception e)
        {
            Debug.Log("Erro ao conectar: " + e.Message);
            statusText.text = "Falha ao conectar";
            SetConnectButton.SetActive(true);
        }
    }

    // LOOP DE LEITURA — agora detecta desconexão real
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
                    string msg = buffer;
                    buffer = "";
                    UnityMainThread(msg);
                }
            }
            catch (Exception e)
            {
                Debug.Log("ERRO NA LEITURA (porta desconectada?): " + e.Message);

                _reading = false;
                _connected = false;

                try { serialPort.Close(); } catch { }

                UnityMainThread("Conexão perdida!");

                return;
            }
        }
    }

    void UnityMainThread(string lostMessage)
    {
        // Executa no próximo Update
        UnityMainThreadDispatcher.Enqueue(() =>
        {
            if (lostMessage != null)
                statusText.text = lostMessage;

            SetConnectButton.SetActive(true);
            Disconnect();
        });
    }

    void ProcessMessage(string msg)
    {
        valuesText.text = msg.Trim();
    }

    void Disconnect()
    {
        _reading = false;
        _connected = false;

        try { readThread?.Abort(); } catch { }

        try
        {
            if (serialPort != null && serialPort.IsOpen)
                serialPort.Close();
        }
        catch { }

        statusText.text = "Desconectado";

        UpdatePortList();
    }

    public void UpdatePortList()
    {
        string[] ports = SerialPort.GetPortNames();

        portDropdown.ClearOptions();
        portDropdown.AddOptions(new List<string>(ports));

        portListText.text = "Portas detectadas:\n" + string.Join("\n", ports);
    }
}