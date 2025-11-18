using UnityEngine;
using static UnityEngine.SceneManagement.SceneManager;

// Controla ações de botões na interface, utilizando o padrão Command
public class ButtonController : MonoBehaviour
{
    [Header("Configuração")]
    // Índice do Wii Remote conectado à Balance Board
    public static int remoteIndex = 0;

    // Botão para iniciar o jogo
    public void PlayButton()
    {
        // Padrão Command: cria um comando para carregar a cena "Jogo"
        ICommand play = new LoadSceneCommand("Jogo");
        play.Execute();    
    }

    // Botão para abrir a tela de configuração
    public void ConfigButton()
    {
        // Padrão Command: comando para carregar a cena "Config"
        ICommand config = new LoadSceneCommand("Config");
        config.Execute();      
    }

    // Botão para abrir o menu de pausa
    public void PauseButton()
    {
        // Padrão Command: comando para carregar a cena "Pause"
        ICommand pause = new LoadSceneCommand("Pause");
        pause.Execute();    
    }

    // Botão para desconectar o Wii Remote atual
    public void DropButton()
    {
        Wii.DropWiiRemote(remoteIndex);
    }

    // Botão para ir à cena de calibração da Balance Board
    public void CalibrationButton()
    {
        // Padrão Command: comando para carregar a cena "Calibração"
        ICommand Calibration = new LoadSceneCommand("Calibração");
        Calibration.Execute();    
    }

    // Botão para reiniciar o jogo
    public void RestartButton()
    {
        StopWatch.ResetTimer(); // Reinicia o cronômetro
        Time.timeScale = 1f;    // Garante que o jogo está despausado

        // Padrão Command: comando para carregar novamente a cena "Jogo"
        ICommand restart = new LoadSceneCommand("Jogo");
        restart.Execute();
    }

    // Botão para voltar ao menu principal
    public void QuitButtom()
    {
        Time.timeScale = 1f; // Garante que o tempo está normal caso estivesse pausado

        // Padrão Command: comando para carregar a cena "Menu"
        ICommand quit = new LoadSceneCommand("Menu");
        quit.Execute();
    }
    
    public void ExitGameButton()
    {
        Debug.Log("Saindo do jogo..."); // Só pra testar no Editor
        Application.Quit();
    }
}