using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StopWatch : MonoBehaviour
{
    public GameObject player;
    public TMP_Text textoTempo; // Arraste um Text da UI aqui no Inspector
    private float tempo = 0f;
    

    void Update()
    {
        if (player == null) // quando o personagem for destruído
        {
            Time.timeScale = 0f; // pausa o tempo global
        }

        tempo += Time.deltaTime;
        int minutos = Mathf.FloorToInt(tempo / 60);
        int segundos = Mathf.FloorToInt(tempo % 60);

        textoTempo.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }
}
