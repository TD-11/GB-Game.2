using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StopWatch : MonoBehaviour, ITimeSubject
{
    public TMP_Text textTimeHud;
    public GameObject player;
    public GameObject gameOverScreen;

    public Color normalColor = Color.white;
    public Color alertColor = Color.red;

    private static float startTime = 120f;
    private static float restTime;
    public float tempoDeAviso = 11f;
    private bool activeTime = true;

    // Lista de observadores (quem vai reagir ao tempo)
    private List<ITimeObserver> observers = new List<ITimeObserver>();

    void Start()
    {
        restTime = startTime;
    }

    void Update()
    {
        if (!activeTime) return;

        if (player == null)
        {
            activeTime = false;
            NotifyTimeEnded();
            return;
        }

        if (restTime > 0)
        {
            restTime -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(restTime / 60);
            int seconds = Mathf.FloorToInt(restTime % 60);

            textTimeHud.text = $"{minutes:00}:{seconds:00}";
            textTimeHud.color = restTime <= tempoDeAviso ? alertColor : normalColor;

            // Notifica todos os observadores da mudança
            NotifyTimeChanged(restTime);
        }
        else
        {
            textTimeHud.text = "00:00";
            textTimeHud.color = alertColor;
            Time.timeScale = 0f;
            gameOverScreen.SetActive(true);
            NotifyTimeEnded();
        }
    }

    public static void ResetTimer()
    {
        restTime = startTime;
    }

    // Padrão Observer
    public void AddObserver(ITimeObserver observer)
    {
        if (!observers.Contains(observer))
            observers.Add(observer);
    }

    public void RemoveObserver(ITimeObserver observer)
    {
        if (observers.Contains(observer))
            observers.Remove(observer);
    }

    public void NotifyTimeChanged(float timeLeft)
    {
        foreach (var observer in observers)
            observer.OnTimeChanged(timeLeft);
    }

    public void NotifyTimeEnded()
    {
        foreach (var observer in observers)
            observer.OnTimeEnded();
    }
}