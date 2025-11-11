using UnityEngine;
using static UnityEngine.SceneManagement.SceneManager;

// Padrão Commander
public class LoadSceneCommand : ICommand
{
    private string sceneName;

    public LoadSceneCommand(string sceneName)
    {
        this.sceneName = sceneName;
    }

    public void Execute()
    {
        Time.timeScale = 1f;
        LoadScene(sceneName);
        Debug.Log($"Cena carregada: {sceneName}");
    }
}