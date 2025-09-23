using UnityEngine;
using static UnityEngine.SceneManagement.SceneManager;

public class ButtonController : MonoBehaviour
{
    public void PlayButton()
    {
        LoadScene("Jogo");// Vai para o jogo
    }
    public void RestartButton()
    {
        StopWatch.ResetTimer();// Restaura o cronômetro
        Time.timeScale = 1f;// Despausa a cena
        LoadScene("Jogo");// Vai para o jogo
    }
    
    public void QuitButtom()
    {
        Time.timeScale = 1f; // Despausa a cena
        LoadScene("Menu");// Vai para o menu
    } 
}
