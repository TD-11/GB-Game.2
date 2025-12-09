using TMPro;
using UnityEngine;
using static Wii;


public class ConnectSDBalance : MonoBehaviour
{
    public TMP_Text connectText;
    public Color normalColor = Color.green;// Define a cor normal do aviso
    public Color alertColor = Color.red;// Define a cor de aviso do aviso
    

    void Update()
    {
        CheckConnection();
    }
    
    public void CheckConnection()
    {
        // Verifica se o acessório conectado é uma Balance Board (tipo 3)
        if (SD_Serial._connected == true)
        {
            connectText.text = "SD-Balance conectado!";
            connectText.color = normalColor;
        }
        
        else
        {
            connectText.text = "SD-Balance desconectado!";
            connectText.color = alertColor;
        }
    }
}