using UnityEngine;
using static UnityEngine.SceneManagement.SceneManager;

public class ButtonController : MonoBehaviour
{
    [Header("Configuração")]
    public static int remoteIndex = 0;// Indice do Wii Remote conectado à Balance Board

    public void PlayButton()
    {
        // Padrão Commander
        ICommand play = new LoadSceneCommand("Jogo");
        play.Execute();    
    }

    public void ConfigButton()
    {
        // Padrão Commander
        ICommand config = new LoadSceneCommand("Config");
        config.Execute();      
    }

    public void PauseButton()
    {
        // Padrão Commander
        ICommand pause = new LoadSceneCommand("Pause");
        pause.Execute();    
    }
    
    public void DropButton()
    {
        Wii.DropWiiRemote(remoteIndex);
    }
    public void CalibrationButton()
    {
        // Padrão Commander
        ICommand Calibration = new LoadSceneCommand("Calibração");
        Calibration.Execute();    
    }
    public void RestartButton()
    {
        StopWatch.ResetTimer();// Restaura o cronômetro
        Time.timeScale = 1f;// Despausa a cena
        // Padrão Commander
        ICommand restart = new LoadSceneCommand("Jogo");
        restart.Execute();
    }
    
    public void QuitButtom()
    {
        Time.timeScale = 1f; // Despausa a cena
        // Padrão Commander
        ICommand quit = new LoadSceneCommand("Menu");
        quit.Execute();
    } 
}
