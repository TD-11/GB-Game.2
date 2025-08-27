using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StopWatch : MonoBehaviour
{
    // Armazenam os game objects e textos
    public TMP_Text textTimeHud;
    public TMP_Text textTotalTime;
    public GameObject player;
    public GameObject timeOutScreen;
    
    public Color normalColor = Color.white;// Define a cor normal do cronômetro
    public Color alertColor = Color.red;// Define a cor de aviso do cronômetro
    public float restTime = 30f; // Tempo em segundos
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
            
            // Enquanto o tempo for maior que zero
            if (restTime > 0)
            {
                restTime -= Time.deltaTime;// Diminui o tempo do cronômetro
                int minutes = Mathf.FloorToInt(restTime / 60);// Converte o valor em minutos
                int seconds = Mathf.FloorToInt(restTime % 60);// Converte o valor em segundos

                // Salva no texto da interface e converte no modelo 00:00
                textTimeHud.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            
                // Quando o tempo chegar no momento de aviso
                if (restTime <= tempoDeAviso)
                {
                    textTimeHud.color = alertColor;// Muda a cor do texto
                }
                else
                {
                    textTimeHud.color = normalColor;// Muda a cor do texto
                }
            }
            // Quando o tempo esgotar
            if (restTime <= 0)
            {
                textTimeHud.text = "00:00";// O texto na tela irá zerar
                textTimeHud.color = alertColor;// A cor se manterá vermelha
                Time.timeScale = 0f;
                timeOutScreen.SetActive(true);// Ativa a tela de tempo esgotado
            }
        }
        
    }
}
