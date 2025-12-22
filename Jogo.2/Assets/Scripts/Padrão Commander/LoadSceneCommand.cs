using UnityEngine;
// Importa estaticamente o SceneManager para permitir chamadas diretas a LoadScene
using static UnityEngine.SceneManagement.SceneManager;

// Implementação do padrão de projeto Command
// Esta classe encapsula a ação de carregar uma cena
public class LoadSceneCommand : ICommand
{
    // Nome da cena que será carregada
    private string sceneName;

    // Construtor que recebe o nome da cena
    public LoadSceneCommand(string sceneName)
    {
        this.sceneName = sceneName;
    }

    // Método exigido pela interface ICommand
    // Executa a ação encapsulada pelo comando
    public void Execute()
    {
        // Garante que o jogo não esteja pausado ao trocar de cena
        Time.timeScale = 1f;

        // Carrega a cena informada
        LoadScene(sceneName);

        // Exibe no console qual cena foi carregada (útil para depuração)
        Debug.Log($"Cena carregada: {sceneName}");
    }
}