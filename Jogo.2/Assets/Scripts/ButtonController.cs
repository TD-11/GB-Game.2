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
        Time.timeScale = 1f;
        LoadScene("Jogo");
    }
    
    public void QuitButtom()
    {
        Time.timeScale = 1f;
        LoadScene("Menu");
    } 
}
