using UnityEngine;
using static UnityEngine.SceneManagement.SceneManager;

public class ButtonController : MonoBehaviour
{
    public void PlayButton()
    {
        LoadScene("Jogo");
    }
    public void RestartButton()
    {
        StopWatch.ResetTimer();
        Time.timeScale = 1f;
        Debug.Log("Tempo voltou");
        LoadScene("Jogo");
    }
    
    public void QuitButtom()
    {
        Time.timeScale = 1f;
        LoadScene("Menu");
    } 
}
