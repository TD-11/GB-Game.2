using UnityEngine;
// Importa estaticamente o SceneManager para facilitar chamadas relacionadas a cenas
using static UnityEngine.SceneManagement.SceneManager;

// Classe responsável por controlar os botões da interface do jogo
public class ButtonController : MonoBehaviour
{
    [Header("Configuração")]

    // Índice do Wii Remote conectado à Balance Board
    // Static para que o valor seja compartilhado entre cenas
    public static int remoteIndex = 0;

    // Método chamado ao pressionar o botão "Jogar"
    public void PlayButton()
    {
        // Implementação do padrão Command
        // Cria um comando para carregar a cena "Jogo"
        ICommand play = new LoadSceneCommand("Jogo");

        // Executa o comando
        play.Execute();    
    }

    // Método chamado ao pressionar o botão "Configurações"
    public void ConfigButton()
    {
        // Padrão Command para carregar a cena de configuração da balança
        ICommand config = new LoadSceneCommand("Config - SD Balance");
        config.Execute();      
    }

    // Método chamado ao pressionar o botão "Pausar"
    public void PauseButton()
    {
        // Padrão Command para carregar a cena de pausa
        ICommand pause = new LoadSceneCommand("Pause");
        pause.Execute();    
    }
    
    // Método chamado ao pressionar o botão para desconectar o Wii Remote
    public void DropButton()
    {
        // Desconecta o Wii Remote usando o índice configurado
        Wii.DropWiiRemote(remoteIndex);
    }

    // Método chamado ao pressionar o botão "Calibração"
    public void CalibrationButton()
    {
        // Padrão Command para carregar a cena de calibração
        ICommand Calibration = new LoadSceneCommand("Calibração");
        Calibration.Execute();    
    }

    // Método chamado ao pressionar o botão "Reiniciar"
    public void RestartButton()
    {
        // Reseta o cronômetro do jogo
        StopWatch.ResetTimer();

        // Garante que o jogo não fique pausado
        Time.timeScale = 1f;

        // Padrão Command para reiniciar a cena principal
        ICommand restart = new LoadSceneCommand("Jogo");
        restart.Execute();
    }
    
    // Método chamado ao pressionar o botão "Sair"
    public void QuitButtom()
    {
        // Garante que o jogo esteja despausado
        Time.timeScale = 1f;

        // Padrão Command para retornar ao menu principal
        ICommand quit = new LoadSceneCommand("Menu");
        quit.Execute();
    } 
}