using TMPro;
using UnityEngine;
using static Wii;


public class ConnectBalance : MonoBehaviour
{
    [Header("Configuração da Balança")]
    public static int remoteIndex = 0; // Índice do Wii Remote conectado
    
    public TMP_Text connectText;
    public Color normalColor = Color.green;// Define a cor normal do aviso
    public Color alertColor = Color.red;// Define a cor de aviso do aviso
    
    public GameObject errorTextInstance;
    public static GameObject errorText;

    void Awake()
    {
        errorText = errorTextInstance;
    }

    void Update()
    {
        CheckConnection();
    }
    
    public void CheckConnection()
    {
        if(Wii.IsActive(remoteIndex))
        {
            // Verifica se o acessório conectado é uma Balance Board (tipo 3)
            if (Wii.GetExpType(remoteIndex) == 3)
            {
                connectText.text = "Balance Board conectado!";
                connectText.color = normalColor;
            }
        }
        
        else
        {
            connectText.text = "Balance Board desconectado!";
            connectText.color = alertColor;
        }
    }
}
