using TMPro;
using UnityEngine;

public class ConnectSDBalance : MonoBehaviour
{
    public TMP_Text connectStartText;
    public TMP_Text connectConfigText;
    
    public Color normalColor = Color.green;// Define a cor normal do aviso
    public Color alertColor = Color.red;// Define a cor de aviso do aviso

    void Update()
    {
        CheckConnection();
    }
    
    public void CheckConnection()
    {
        // Verifica se o acessório conectado é uma Balance Board (tipo 3)
        if (SD_Serial._connected == true && SD_Serial.S != 0)
        {
            connectStartText.text = "SD-Balance conectado!";
            connectStartText.color = normalColor;
            
            connectConfigText.text = "SD-Balance conectado!";
            connectConfigText.color = normalColor;
            Debug.Log("Cheguei");
        }
        
        else
        {
            connectStartText.text = "SD-Balance desconectado!";
            connectStartText.color = alertColor;
            
            connectConfigText.text = "SD-Balance desconectado!";
            connectConfigText.color = alertColor;
        }
    }
}