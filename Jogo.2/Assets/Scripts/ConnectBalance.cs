using TMPro;
using UnityEngine;
using static Wii;


public class ConnectBalance : MonoBehaviour
{
    [Header("Configuração da Balança")]
    public static int remoteIndex = 0; // Índice do Wii Remote conectado
    
    public GameObject connectButton;
    public TMP_Text connectText;
    public Color normalColor = Color.green;// Define a cor normal do aviso
    public Color alertColor = Color.red;// Define a cor de aviso do aviso
    
    void Start()
    {
        CheckConnection();
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
                connectText.text = "Aparelho conectado!";
                connectText.color = normalColor;
                connectButton.SetActive(false);
            }
        }
        
        else
        {
            connectText.text = "Aparelho desconectado!";
            connectText.color = alertColor;
            connectButton.SetActive(true);
        }
    }
}
