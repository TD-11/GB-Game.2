using UnityEngine;
using static UnityEngine.SceneManagement.SceneManager;

public class ButtonController : MonoBehaviour
{
    [Header("Configuração")]
    public static int remoteIndex = 0;// Indice do Wii Remote conectado à Balance Board
    public void PlayButton()
    {
        LoadScene("Jogo");
    }

    public void ConfigButton()
    {
        LoadScene("Config");
    }

    public void PauseButton()
    {
        LoadScene("Pause");
    }

    public void DropButton()
    {
        Wii.DropWiiRemote(remoteIndex);
    }
    public void CalibrationButton()
    {
        LoadScene("Calibração");// Vai para o jogo
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
