using System;
using UnityEngine;
using System.Collections;
using TMPro;
using static Wii;

public class BalanceBoardCalibration : MonoBehaviour
{
    [Header("Configuração da Balança")]
    public static int remoteIndex = 0;
    public float detectionThreshold = 5f;
    public float measureDuration = 5f;

    [Header("Referências de UI")]
    public TMP_Text messageText;
    public TMP_Text countdownText;
    public TMP_Text resultText;
    public GameObject playButton;

    [Header("Resultados da Calibração")]
    public static float playerWeight { get; private set; } = 0f;
    public bool isCalibrating { get; private set; } = false;
    public bool calibrationComplete { get; private set; } = false;

    private bool boardConnected = false; // Novo: guarda o estado atual da balança

    void Start()
    {
        Time.timeScale = 1f;
        CheckConnection();
    }

    void Update()
    {
        // Checa se houve mudança no estado da conexão
        if (Wii.IsActive(remoteIndex) != boardConnected)
        {
            CheckConnection();
        }

        if (boardConnected)
        {
            HandleCalibration();
        }
    }

    void CheckConnection()
    {
        if (Wii.IsActive(remoteIndex) && Wii.GetExpType(remoteIndex) == 3)
        {
            boardConnected = true;
            messageText.text = "Suba no aparelho para iniciar!";
            countdownText.text = "";
            resultText.text = "";
            playButton.SetActive(false);
            Debug.Log("BB: " + Wii.GetExpType(remoteIndex));
        }
        else
        {
            boardConnected = false;
            messageText.text = "Balance Board desconectada!\nModo Manual ativado";
            countdownText.text = "";
            resultText.text = "";
            playButton.SetActive(true);
            Debug.LogWarning("Modo Manual ativado (sem Balance Board)");
        }
    }

    void HandleCalibration()
    {
        float currentWeight = Wii.GetTotalWeight(remoteIndex);

        if (!isCalibrating && !calibrationComplete && currentWeight > detectionThreshold)
        {
            StartCoroutine(CalibratePlayerWeight());
        }
    }

    private IEnumerator CalibratePlayerWeight()
    {
        isCalibrating = true;
        messageText.text = "Calibrando... Mantenha-se parado";
        resultText.text = "Calculando peso:";

        float elapsed = 0f;
        float sum = 0f;
        int samples = 0;

        while (elapsed < measureDuration)
        {
            float w = Wii.GetTotalWeight(remoteIndex);
            sum += w;
            samples++;
            elapsed += Time.unscaledDeltaTime;

            countdownText.text = $"{measureDuration - elapsed:F1}s";

            if (w < detectionThreshold)
            {
                messageText.text = "Jogador saiu do aparelho! Tente novamente";
                countdownText.text = "";
                resultText.text = "";
                isCalibrating = false;
                yield break;
            }

            yield return null;
        }

        playerWeight = (samples > 0) ? (sum / samples) : 0f;
        calibrationComplete = true;
        isCalibrating = false;

        messageText.text = "Ajuste finalizado!";
        countdownText.text = "";
        resultText.text = $"Peso armazenado: {playerWeight:F2} kg";
        playButton.SetActive(true);
    }
}