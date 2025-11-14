using UnityEngine;
using System;
using System.IO.Ports;
using System.Threading;
using TMPro;

public class SD_Serial : MonoBehaviour
{
    public static SD_Serial Instance;

    [Header("UI - Ligado via Inspetor")]
    public string selectedPort = "COM1";
    public TMP_Text statusText; // Texto de status
    public bool autoSelectFirstPort = true;

    [Header("Leituras (somente leitura)")]
    public float A, B, C, D, P;

    private SerialPort serial;
    private Thread readThread;
    private bool isRunning = false;
    private bool allowReading = false; // só libera depois de 30 segundos
    private string buffer = "";

    private void Awake()
    {
        // Singleton + Persistência
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (autoSelectFirstPort && SerialPort.GetPortNames().Length > 0)
            selectedPort = SerialPort.GetPortNames()[0];

        UpdateStatus($"Porta selecionada: {selectedPort}");
    }

    // Atualiza o texto de status
    void UpdateStatus(string msg)
    {
        Debug.Log(msg);
        if (statusText != null)
            statusText.text = msg;
    }

    // Botão do Inspetor
    public void ConnectPort()
    {
        if (isRunning)
        {
            UpdateStatus("Já está conectado!");
            return;
        }

        try
        {
            serial = new SerialPort(selectedPort, 57600);
            serial.Parity = Parity.None;
            serial.StopBits = StopBits.One;
            serial.DataBits = 8;
            serial.Handshake = Handshake.None;

            serial.ReadTimeout = 200;
            serial.WriteTimeout = 200;

            serial.Open();
            isRunning = true;

            UpdateStatus($"Conectado à porta {selectedPort}. Aguarde 30s...");

            // Inicia thread de leitura
            readThread = new Thread(ReadLoop);
            readThread.Start();

            // Só libera leitura depois de 30 segundos
            Invoke(nameof(EnableReading), 30f);
        }
        catch (Exception e)
        {
            UpdateStatus("Erro ao abrir a porta: " + e.Message);
        }
    }

    private void EnableReading()
    {
        allowReading = true;
        UpdateStatus($"Conexão ativa em {selectedPort}. Lendo dados...");
    }

    // Thread de leitura contínua
    private void ReadLoop()
    {
        while (isRunning)
        {
            if (!allowReading) continue;

            try
            {
                char c = (char)serial.ReadByte();
                buffer += c;

                if (c == '\n')
                {
                    ProcessLine(buffer);
                    buffer = "";
                }
            }
            catch { }
        }
    }

    private void ProcessLine(string line)
    {
        line = line.Trim();

        if (!line.StartsWith("*"))
            return;

        line = line.Substring(1); // remove *

        string[] parts = line.Split(';');
        if (parts.Length < 5) return;

        try
        {
            A = float.Parse(parts[0].Replace('.', ','));
            B = float.Parse(parts[1].Replace('.', ','));
            C = float.Parse(parts[2].Replace('.', ','));
            D = float.Parse(parts[3].Replace('.', ','));
            P = float.Parse(parts[4].Replace('.', ','));
        }
        catch (Exception e)
        {
            Debug.Log("Erro parse: " + e.Message);
        }
    }

    public void Disconnect()
    {
        isRunning = false;

        try
        {
            if (serial != null && serial.IsOpen)
                serial.Close();
        }
        catch { }
    }

    private void OnApplicationQuit()
    {
        Disconnect();
    }
}