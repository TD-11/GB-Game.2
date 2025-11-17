using UnityEngine;
using static UnityEngine.SceneManagement.SceneManager;

// Classe concreta do Padrão Command.
// Sua responsabilidade é encapsular a ação de carregar uma cena no Unity.
public class LoadSceneCommand : ICommand
{
    private string sceneName; // Nome da cena que será carregada ao executar o comando

    // Construtor recebe o nome da cena e armazena internamente.
    // Isso permite reutilizar o comando em diferentes botões e momentos do jogo.
    public LoadSceneCommand(string sceneName)
    {
        this.sceneName = sceneName;
    }

    // Método exigido pela interface ICommand.
    // Executa o comportamento associado a este comando: carregar uma cena.
    public void Execute()
    {
        Time.timeScale = 1f;       // Garante que o jogo não está pausado antes da troca de cena
        LoadScene(sceneName);      // Carrega a cena desejada
        Debug.Log($"Cena carregada: {sceneName}"); // Log para depuração
    }
}