using UnityEngine;
using System;
using System.IO.Ports;
using System.Threading;

public class SD_SerialManager : MonoBehaviour
{
    public static SD_SerialManager Instance;

    [Header("Configuração Serial")]
    public int baudRate = 57600;

    [HideInInspector] public bool connected = false;

    private SerialPort serialPort;
    private Thread readThread;

    private bool reading = false;
    private string buffer = "";

    // Valores globais
    public float A, B, C, D, P;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // 🔥 não destruir ao trocar de cena
    }

    // ----------- CONECTAR -----------
    public bool Connect(string portName)
    {
        if (connected) return true;

        try
        {
            serialPort = new SerialPort(portName, baudRate);
            serialPort.Open();

            connected = true;
            StartReading();
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError("Erro ao conectar: " + e.Message);
            return false;
        }
    }

    // ----------- DESCONECTAR -----------
    public void Disconnect()
    {
        if (!connected) return;

        reading = false;
        connected = false;

        try { readThread?.Abort(); } catch { }

        if (serialPort != null && serialPort.IsOpen)
            serialPort.Close();
    }

    // ----------- THREAD DE LEITURA -----------
    void StartReading()
    {
        reading = true;
        readThread = new Thread(ReadLoop);
        readThread.Start();
    }

    void ReadLoop()
    {
        while (reading)
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

    // ----------- PROCESSAR DADOS -----------
    void ProcessMessage(string msg)
    {
        if (!msg.StartsWith("*")) return;

        msg = msg.Substring(1);
        string[] p = msg.Split(';');
        if (p.Length < 5) return;

        A = float.Parse(p[0].Replace('.', ','));
        B = float.Parse(p[1].Replace('.', ','));
        C = float.Parse(p[2].Replace('.', ','));
        D = float.Parse(p[3].Replace('.', ','));
        P = float.Parse(p[4].Replace('.', ','));
    }

    // ----------- QUIT -----------
    void OnApplicationQuit()
    {
        Disconnect();
    }
}