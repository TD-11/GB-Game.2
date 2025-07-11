using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StopWatch : MonoBehaviour
{
    // Armazenam os game objects e textos
    public TMP_Text textTime;
    public GameObject player;
    public GameObject timeOutScreen;
    
    public Color normalColor = Color.white;// Define a cor normal do cronômetro
    public Color alertColor = Color.red;// Define a cor de aviso do cronômetro
    public float restTime = 30f; // tempo em segundos
    public float tempoDeAviso = 11f;// Define a partir de que tempo aparecerá o aviso
    private bool activeTime = true;// Servirá para desativar o tempo
    
    void Update()
    {
        if (activeTime)
        {
            // Caso o player seja derrotado
            if (player == null)
            {
                activeTime = false;
            }
            
            // Enquanto o tempo for maior que 0
            if (restTime > 0)
            {
                restTime -= Time.deltaTime;// Diminui o tempo do cronômetro
                int minutes = Mathf.FloorToInt(restTime / 60);// Converte o valor em minutos
                int seconds = Mathf.FloorToInt(restTime % 60);// Converte o valor em segundos

                // Salva no texto da interface e converte no modelo 00:00
                textTime.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            
                // Quando o tempo chegar no momento de aviso
                if (restTime <= tempoDeAviso)
                {
                    textTime.color = alertColor;// Muda a cor do texto
                }
                else
                {
                    textTime.color = normalColor;// Muda a cor do texto
                }
            }
            // Quando o tempo esgotar
            if (restTime <= 0)
            {
                textTime.text = "00:00";// O texto na tela irá zerar
                textTime.color = alertColor;// A cor se manterá vermelha 
                activeTime = false;// Desativa o tempo
                timeOutScreen.SetActive(true);// Ativa a tela de tempo esgotado
            }
        }
        
    }
}
