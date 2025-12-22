using UnityEngine;
using System;
using System.IO.Ports;
using System.Threading;

public class SD_SerialManager : MonoBehaviour
{
    // =========================
    //        SINGLETON
    // =========================
    // Instância global do gerenciador serial
    public static SD_SerialManager Instance;

    // =========================
    //   CONFIGURAÇÃO SERIAL
    // =========================
    [Header("Configuração Serial")]
    public int baudRate = 57600; // Taxa de comunicação com o dispositivo

    // Indica se a conexão está ativa
    [HideInInspector] public bool connected = false;

    // =========================
    //  CONTROLE DA CONEXÃO
    // =========================
    private SerialPort serialPort; // Porta serial utilizada
    private Thread readThread;     // Thread responsável pela leitura contínua

    private bool reading = false;  // Controla o loop da thread
    private string buffer = "";    // Buffer de dados recebidos

    // =========================
    //    VALORES RECEBIDOS
    // =========================
    // Valores globais lidos da SD-Balance
    public float A, B, C, D, P;

    void Awake()
    {
        // Garante que exista apenas uma instância (Singleton)
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Mantém o gerenciador entre trocas de cena
        DontDestroyOnLoad(gameObject);
    }

    // =========================
    //          CONECTAR
    // =========================
    // Tenta conectar à porta serial informada
    public bool Connect(string portName)
    {
        // Evita reconectar se já estiver conectado
        if (connected) return true;

        try
        {
            // Cria e abre a porta serial
            serialPort = new SerialPort(portName, baudRate);
            serialPort.Open();

            connected = true;

            // Inicia a leitura em background
            StartReading();
            return true;
        }
        catch (Exception e)
        {
            // Exibe erro caso falhe
            Debug.LogError("Erro ao conectar: " + e.Message);
            return false;
        }
    }

    // =========================
    //        DESCONECTAR
    // =========================
    // Encerra a conexão serial
    public void Disconnect()
    {
        if (!connected) return;

        reading = false;
        connected = false;

        // Interrompe a thread de leitura
        try { readThread?.Abort(); } catch { }

        // Fecha a porta serial
        if (serialPort != null && serialPort.IsOpen)
            serialPort.Close();
    }

    // =========================
    //    THREAD DE LEITURA
    // =========================
    // Inicia a thread de leitura
    void StartReading()
    {
        reading = true;
        readThread = new Thread(ReadLoop);
        readThread.Start();
    }

    // Loop contínuo de leitura da serial
    void ReadLoop()
    {
        while (reading)
        {
            try
            {
                // Lê byte por byte
                char c = (char)serialPort.ReadByte();
                buffer += c;

                // Fim da mensagem
                if (c == '\n')
                {
                    ProcessMessage(buffer);
                    buffer = "";
                }
            }
            catch
            {
                // Ignora erros momentâneos de leitura
            }
        }
    }

    // =========================
    //    PROCESSAR DADOS
    // =========================
    // Interpreta a mensagem recebida
    void ProcessMessage(string msg)
    {
        // Mensagens válidas começam com '*'
        if (!msg.StartsWith("*")) return;

        // Remove o marcador inicial
        msg = msg.Substring(1);

        // Divide os valores
        string[] p = msg.Split(';');
        if (p.Length < 5) return;

        // Converte os valores recebidos
        A = float.Parse(p[0].Replace('.', ','));
        B = float.Parse(p[1].Replace('.', ','));
        C = float.Parse(p[2].Replace('.', ','));
        D = float.Parse(p[3].Replace('.', ','));
        P = float.Parse(p[4].Replace('.', ','));
    }

    // =========================
    //        SAÍDA
    // =========================
    void OnApplicationQuit()
    {
        // Garante que a porta seja fechada ao sair do jogo
        Disconnect();
    }
}