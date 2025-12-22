using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Classe responsável por controlar o cronômetro do jogo
// Implementa o padrão Observer através da interface ITimeSubject
public class StopWatch : MonoBehaviour, ITimeSubject
{
    // =========================
    // REFERÊNCIAS DE UI
    // =========================

    // Texto do HUD que exibe o tempo restante
    public TMP_Text textTimeHud;

    // Referência ao jogador (usada para verificar se ele ainda está vivo)
    public GameObject player;

    // Tela de Game Over
    public GameObject gameOverScreen;

    // =========================
    // CONFIGURAÇÃO VISUAL
    // =========================

    // Cor padrão do texto do tempo
    public Color normalColor = Color.white;

    // Cor de alerta quando o tempo estiver acabando
    public Color alertColor = Color.red;

    // =========================
    // CONTROLE DO TEMPO
    // =========================

    // Tempo inicial do cronômetro (em segundos)
    private static float startTime = 120f;

    // Tempo restante
    private static float restTime;

    // Tempo (em segundos) para ativar o aviso visual
    public float tempoDeAviso = 11f;

    // Controla se o cronômetro está ativo
    private bool activeTime = true;

    // =========================
    // PADRÃO OBSERVER
    // =========================

    // Lista de observadores que reagem às mudanças de tempo
    private List<ITimeObserver> observers = new List<ITimeObserver>();

    // =========================
    // MÉTODOS UNITY
    // =========================

    void Start()
    {
        // Inicializa o tempo restante com o tempo inicial
        restTime = startTime;
    }

    void Update()
    {
        // Se o cronômetro não estiver ativo, não executa a lógica
        if (!activeTime) return;

        // Se o jogador não existir mais (morreu)
        if (player == null)
        {
            activeTime = false;
            NotifyTimeEnded();
            return;
        }

        // Se ainda houver tempo restante
        if (restTime > 0)
        {
            // Reduz o tempo restante com base no tempo real
            restTime -= Time.deltaTime;

            // Converte segundos em minutos e segundos
            int minutes = Mathf.FloorToInt(restTime / 60);
            int seconds = Mathf.FloorToInt(restTime % 60);

            // Atualiza o texto do HUD
            textTimeHud.text = $"{minutes:00}:{seconds:00}";

            // Altera a cor quando entra no tempo de aviso
            textTimeHud.color = restTime <= tempoDeAviso ? alertColor : normalColor;

            // Notifica todos os observadores sobre a mudança de tempo
            NotifyTimeChanged(restTime);
        }
        else
        {
            // Quando o tempo acaba
            textTimeHud.text = "00:00";
            textTimeHud.color = alertColor;

            // Pausa o jogo
            Time.timeScale = 0f;

            // Ativa a tela de Game Over
            gameOverScreen.SetActive(true);

            // Notifica os observadores que o tempo acabou
            NotifyTimeEnded();
        }
    }

    // =========================
    // MÉTODOS PÚBLICOS
    // =========================

    // Reseta o cronômetro para o tempo inicial
    public static void ResetTimer()
    {
        restTime = startTime;
    }

    // =========================
    // IMPLEMENTAÇÃO DO OBSERVER
    // =========================

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

    // Notifica os observadores quando o tempo muda
    public void NotifyTimeChanged(float timeLeft)
    {
        foreach (var observer in observers)
            observer.OnTimeChanged(timeLeft);
    }

    // Notifica os observadores quando o tempo acaba
    public void NotifyTimeEnded()
    {
        foreach (var observer in observers)
            observer.OnTimeEnded();
    }
}