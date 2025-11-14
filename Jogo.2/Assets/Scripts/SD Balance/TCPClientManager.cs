using UnityEngine;
using System.Net.Sockets;
using System.Text;

public class TCPClientManager : MonoBehaviour
{
    public static TCPClientManager Instance;

    private TcpClient client;
    private NetworkStream stream;

    [Header("Configuração")]
    public string ip = "255.255.0.0";
    public int port = 8080;

    private void Awake()
    {
        // Singleton básico
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // 🔥 NÃO DESTRUIR AO MUDAR DE CENA
        DontDestroyOnLoad(gameObject);
    }

    public bool Connect()
    {
        try
        {
            client = new TcpClient();
            client.Connect(ip, port);
            stream = client.GetStream();
            Debug.Log("Conectado ao servidor!");
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Erro ao conectar: " + e.Message);
            return false;
        }
    }

    public void SendMessageTCP(string mensagem)
    {
        if (stream == null) return;
        byte[] data = Encoding.UTF8.GetBytes(mensagem);
        stream.Write(data, 0, data.Length);
    }

    public void Disconnect()
    {
        try
        {
            stream?.Close();
            client?.Close();
        }
        catch {}

        stream = null;
        client = null;
    }

    private void OnApplicationQuit()
    {
        Disconnect();
    }
}