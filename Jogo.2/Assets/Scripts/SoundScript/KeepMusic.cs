using UnityEngine;
using UnityEngine.SceneManagement;

public class KeepMusic : MonoBehaviour
{
    public AudioSource audioSource; // arraste sua música aqui no Inspector
    public static KeepMusic instance;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        if ((SceneManager.GetActiveScene().name == "Menu" || SceneManager.GetActiveScene().name == "Calibração" || SceneManager.GetActiveScene().name == "Config")
            && audioSource.isPlaying==false)
        {
            audioSource.Play();
        }
        
        if (SceneManager.GetActiveScene().name == "Jogo" && audioSource.isPlaying ==true)
        {
            audioSource.Stop();
        }
    }
}