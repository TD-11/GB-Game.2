using TMPro;
using UnityEngine;

public class ConnectSDBalance : MonoBehaviour
{
    // Texto exibido na tela inicial informando o status da conexão
    public TMP_Text connectStartText;

    // Texto exibido na tela de configuração informando o status da conexão
    public TMP_Text connectConfigText;
    
    // Cor utilizada quando a SD-Balance está conectada
    public Color normalColor = Color.green;

    // Cor utilizada quando a SD-Balance está desconectada
    public Color alertColor = Color.red;

    void Update()
    {
        // Verifica constantemente o estado da conexão
        CheckConnection();
    }
    
    public void CheckConnection()
    {
        // Verifica se:
        // 1) A conexão serial está ativa (_connected == true)
        // 2) O valor S é diferente de zero (indicando resposta do dispositivo)
        if (SD_Serial._connected == true && SD_Serial.S != 0)
        {
            // Atualiza o texto e a cor indicando que o dispositivo está conectado
            connectStartText.text = "SD-Balance conectado!";
            connectStartText.color = normalColor;
            
            connectConfigText.text = "SD-Balance conectado!";
            connectConfigText.color = normalColor;

            // Log apenas para depuração
            Debug.Log("SD-Balance conectado");
        }
        else
        {
            // Atualiza o texto e a cor indicando que o dispositivo está desconectado
            connectStartText.text = "SD-Balance desconectado!";
            connectStartText.color = alertColor;
            
            connectConfigText.text = "SD-Balance desconectado!";
            connectConfigText.color = alertColor;
        }
    }
}