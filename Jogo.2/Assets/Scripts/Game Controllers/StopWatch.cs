using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Classe que funciona como um cronômetro regressivo (StopWatch).
// Também atua como "Subject" no padrão Observer, notificando objetos interessados nas mudanças do tempo.
public class StopWatch : MonoBehaviour, ITimeSubject
{
    public TMP_Text textTimeHud;       // Texto no HUD que exibe o tempo restante
    public GameObject player;          // Referência ao jogador (serve para saber quando o player morre)
    public GameObject gameOverScreen;  // Tela exibida quando o tempo acaba ou o player morre

    public Color normalColor = Color.white;  // Cor padrão da fonte do HUD
    public Color alertColor = Color.red;     // Cor usada quando o tempo está baixo

    private static float startTime = 120f;   // Tempo inicial (120 segundos)
    private static float restTime;           // Tempo restante (regressivo)
    public float tempoDeAviso = 11f;         // Limite para mudar para cor de alerta no HUD
    private bool activeTime = true;          // Controla se o cronômetro está ativo

    // Lista de observadores — qualquer objeto interessado em receber atualizações do tempo
    private List<ITimeObserver> observers = new List<ITimeObserver>();


    void Start()
    {
        // Inicializa o cronômetro com o tempo padrão
        restTime = startTime;
    }


    void Update()
    {
        // Se o tempo está pausado, não faz nada
        if (!activeTime) return;

        // Se o jogador for destruído/perdido, o cronômetro pára e avisa os observadores
        if (player == null)
        {
            activeTime = false;
            NotifyTimeEnded();
            return;
        }

        // Enquanto ainda tiver tempo sobrando
        if (restTime > 0)
        {
            restTime -= Time.deltaTime; // Subtrai o tempo proporcional ao frame

            // Converte o tempo para formato MM:SS
            int minutes = Mathf.FloorToInt(restTime / 60);
            int seconds = Mathf.FloorToInt(restTime % 60);

            // Atualiza o HUD
            textTimeHud.text = $"{minutes:00}:{seconds:00}";
            textTimeHud.color = restTime <= tempoDeAviso ? alertColor : normalColor;

            // Notifica todos os observadores da mudança de tempo
            NotifyTimeChanged(restTime);
        }
        else
        {
            // Caso o tempo chegue a 0
            textTimeHud.text = "00:00";
            textTimeHud.color = alertColor;

            Time.timeScale = 0f;           // Pausa o jogo
            gameOverScreen.SetActive(true); // Abre a tela de Game Over

            NotifyTimeEnded();             // Notifica observadores que o tempo acabou
        }
    }


    // Reinicia o cronômetro (método estático para ser acessado de qualquer script)
    public static void ResetTimer()
    {
        restTime = startTime;
    }


    // --------------------------
    // Padrão Observer (Subject)
    // --------------------------

    // Adiciona um observador à lista
    public void AddObserver(ITimeObserver observer)
    {
        if (!observers.Contains(observer))
            observers.Add(observer);
    }

    // Remove um observador da lista
    public void RemoveObserver(ITimeObserver observer)
    {
        if (observers.Contains(observer))
            observers.Remove(observer);
    }

    // Notifica todos os observadores que o tempo mudou
    public void NotifyTimeChanged(float timeLeft)
    {
        foreach (var observer in observers)
            observer.OnTimeChanged(timeLeft);
    }

    // Notifica todos os observadores que o tempo acabou
    public void NotifyTimeEnded()
    {
        foreach (var observer in observers)
            observer.OnTimeEnded();
    }
}