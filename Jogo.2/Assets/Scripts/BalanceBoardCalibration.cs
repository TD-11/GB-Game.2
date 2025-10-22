using System;
using UnityEngine;
using System.Collections;
using TMPro;
using static Wii;

// Biblioteca da Balance Board

public class BalanceBoardCalibration : MonoBehaviour
{
    [Header("Configuração da Balança")]
    public static int remoteIndex = 0; // Índice do Wii Remote conectado
    public float detectionThreshold = 5f; // Peso mínimo para detectar que o jogador subiu
    public float measureDuration = 5f; // Tempo de medição (em segundos)

    [Header("Referências de UI")]
    public TMP_Text messageText;   // Texto principal de status (ex: "Suba na balança")
    public TMP_Text countdownText; // Texto para contagem regressiva
    public TMP_Text resultText;    // Texto para exibir o peso final
    public GameObject playButton;

    [Header("Resultados da Calibração")]
    public static float playerWeight { get; private set; } = 0f;
    public bool isCalibrating { get; private set; } = false;
    public bool calibrationComplete { get; private set; } = false;



    void Start()
    {
     
        
        // Garante que o jogo não está pausado
        Time.timeScale = 1f;

       /*
        if(Wii.IsActive(remoteIndex))
        {
            Wii.DropWiiRemote(0);
        }
       */
       
       // Wii.StartSearch();

        if (!Wii.IsActive(remoteIndex))
        {
            messageText.text = "Modo Manual.";
            resultText.text = "";
            countdownText.text = "";
           // messageText.text = "Balance Board não conectada!";
            Debug.LogError("Balance Board não está ativa ou conectada!");
           // return;
        }
 

        messageText.text = "Suba na no aparelho para iniciar!";
        countdownText.text = "";
        resultText.text = "";
        playButton.SetActive(false);
    }

  /*
    float elapsed = 0f;
    
    
    private void FixedUpdate()
    {
        Debug.unityLogger.Log("EL: " + elapsed);

       // Verifica se o acessório conectado é uma Balance Board (tipo 3)
       if (Wii.GetExpType(remoteIndex) == 3)
       {

           //=====================================

           float currentWeight = Wii.GetTotalWeight(remoteIndex);
           // Quando o jogador sobe e ainda não calibramos
           if (!isCalibrating && !calibrationComplete && currentWeight > detectionThreshold)
           {

               isCalibrating = true;
               messageText.text = "Calibrando... Mantenha-se parado.";
               resultText.text = "Calculando peso:";

           
               float sum = 0f;
               int samples = 0;

               if (elapsed < measureDuration)
               {
                   float w = Wii.GetTotalWeight(remoteIndex);
                   sum += w;
                   samples++;

                   elapsed += Time.unscaledDeltaTime;
                   Debug.unityLogger.Log("EL2: " + elapsed);

                   // Contagem regressiva na tela
                   float remaining = measureDuration - elapsed;
                   countdownText.text = $"{remaining:F1}s";

                   // Caso o jogador saia antes do tempo acabar
                   if (w < detectionThreshold)
                   {
                       messageText.text = "Jogador saiu do aparelho! Tente novamente.";
                       resultText.text = "";
                       countdownText.text = "";
                       isCalibrating = false;

                   }
               }
               else
               {

                   elapsed = 0;
               }

               // Calcula peso médio
                   playerWeight = (samples > 0) ? (sum / samples) : 0f;
                   calibrationComplete = true;
                   isCalibrating = false;

                   messageText.text = "Ajuste finalizado!";
                   countdownText.text = "";
                   resultText.text = $"Peso armazenado: {playerWeight:F2} kg";
                   playButton.SetActive(true);


                   Debug.Log($"Peso armazenado: {playerWeight:F2} kg");
               

           }


       }
       
    }

*/
  
    void Update()
    {
        // Verifica se o acessório conectado é uma Balance Board (tipo 3)
        if (Wii.GetExpType(remoteIndex) == 3)
        {
            float currentWeight = Wii.GetTotalWeight(remoteIndex);

            // Quando o jogador sobe e ainda não calibramos
            if (!isCalibrating && !calibrationComplete && currentWeight > detectionThreshold)
            {
                StartCoroutine(CalibratePlayerWeight());
               
            }
        }
        else
        {
            playButton.SetActive(true);
            
        }
    }

    private IEnumerator CalibratePlayerWeight()
    {
        isCalibrating = true;
        messageText.text = "Calibrando... Mantenha-se parado.";
        resultText.text = "Calculando peso:";

        float elapsed = 0f;
        float sum = 0f;
        int samples = 0;

        
        if ( measureDuration < elapsed || !Wii.IsActive(remoteIndex) )
        {
           // messageText.text = "Modo Manual.";
            playButton.SetActive(false);
            yield return null;
        }
        
        
        while (elapsed < measureDuration)
        {
            float w = Wii.GetTotalWeight(remoteIndex);
            sum += w;
            samples++;

            elapsed += Time.unscaledDeltaTime;

            // Contagem regressiva na tela
            float remaining = measureDuration - elapsed;
            countdownText.text = $"{remaining:F1}s";

            // Caso o jogador saia antes do tempo acabar
            if ( w < detectionThreshold)
            {
                messageText.text = "Jogador saiu do aparelho! Tente novamente.";
                resultText.text = "";
                countdownText.text = "";
                isCalibrating = false;
                yield break;
            }

            yield return null;
        }




        // Calcula peso médio
        playerWeight = (samples > 0) ? (sum / samples) : 0f;
        calibrationComplete = true;
        isCalibrating = false;

        messageText.text = "Ajuste finalizado!";
        countdownText.text = "";
        resultText.text = $"Peso armazenado: {playerWeight:F2} kg";
        playButton.SetActive(true);


        Debug.Log($"Peso armazenado: {playerWeight:F2} kg");
    }

    public void ResetCalibration()
    {
        playerWeight = 0f;
        calibrationComplete = false;
        isCalibrating = false;

        messageText.text = "Suba na no aparelho para iniciar!.";
        countdownText.text = "";
        resultText.text = "";
        playButton.SetActive(false);

    }
}