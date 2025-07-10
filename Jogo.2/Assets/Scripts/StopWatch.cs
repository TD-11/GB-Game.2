using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StopWatch : MonoBehaviour
{
    public TMP_Text textTime;
    public GameObject player;
    public float restTime = 60f; // tempo em segundos
    public Color normalColor = Color.white;
    public Color alertColor = Color.red;
    public float tempoDeAviso = 11f;
    private bool activeTime = true;

    void Update()
    {
        if (player == null)
        {
        activeTime = false;
        }

        if (activeTime)
        {
            restTime -= Time.deltaTime;
            int minutos = Mathf.FloorToInt(restTime / 60);
            int segundos = Mathf.FloorToInt(restTime % 60);

            textTime.text = string.Format("{0:00}:{1:00}", minutos, segundos);
            
            if (restTime <= tempoDeAviso)
            {
                textTime.color = alertColor;
            }
            else
            {
                textTime.color = normalColor;
            }
        }
        else
        {
            textTime.text = "00:00";
            textTime.color = alertColor; // Cor final
            Time.timeScale = 0f;
            // Aqui você pode chamar Game Over ou qualquer outra lógica
        }
    }
}
