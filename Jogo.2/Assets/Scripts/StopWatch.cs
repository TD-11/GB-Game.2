using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StopWatch : MonoBehaviour
{
    public GameObject player;
    public TMP_Text textoTempo; 
    private float tempo = 0f;
    

    void Update()
    {
        if (player == null) // Quando o personagem for destruído
        {
            Time.timeScale = 0f; // Pausa o tempo global
        }

        tempo += Time.deltaTime;
        int minutos = Mathf.FloorToInt(tempo / 60);// Converte o tempo em minutos
        int segundos = Mathf.FloorToInt(tempo % 60);// Converte o tempo em segundos

        textoTempo.text = string.Format("{0:00}:{1:00}", minutos, segundos);// espõe os valores no texto da interface
    }
}
