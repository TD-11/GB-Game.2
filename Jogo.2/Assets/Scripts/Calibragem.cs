using System.Collections;
using TMPro;
using UnityEngine;

public class Calibragem : MonoBehaviour
{
    [Header("Configuração")]
    public static int remoteIndex = 0;// Indice do Wii Remote conectado à Balance Board
    
    // Valor final salvo (média das leituras em 5s)
    public float patientWeight { get; private set; } = 0f;

    // Controle de estado
    public bool isMeasuring { get; private set; } = false;
    public bool weightSaved { get; private set; } = false;

    [Header("Configuração de Medição")]
    public float detectionThreshold = 5f;   // Mínimo para considerar que alguém subiu
    public float measureDuration = 5f;      // Tempo em segundos (5s)
    
    float totalWeight = Wii.GetTotalWeight(remoteIndex);// Variável para contagem do peso total
    
    void Start()
    {
        if (!Wii.IsActive(remoteIndex))
        {
            return;
        }
        
        print("Iniciando " + Time.time );
    }

    // Update is called once per frame
    void Update()
    {
        if (!Wii.IsActive(remoteIndex))
        {
            return;
        }
        
        // Usa o método da biblioteca para pegar o peso total
        float currentWeight = totalWeight;

        // Detecta se o paciente subiu e ainda não mediu
        if (!isMeasuring && !weightSaved && currentWeight > detectionThreshold)
        {
            StartCoroutine(MeasureAverageWeight());
        }
    }
    
    private IEnumerator MeasureAverageWeight()
    {
        isMeasuring = true;// Sinaliza que está medindo
        float elapsed = 0f;// Tempo acumulado
        float sum = 0f;    // Soma dos valores de peso lidos
        int samples = 0;   // Número de leituras feitas

        Debug.Log($"Paciente detectado. Medindo por {measureDuration} segundos...");

        // Enquanto não atingir o tempo definido de medição
        while (elapsed < measureDuration)
        {
            float w = totalWeight;// Lê peso atual

            sum += w; // Acumula peso para depois tirar média
            samples++;// Conta mais uma leitura

            elapsed += Time.unscaledDeltaTime;// Incrementa tempo com base no tempo real (não afetado por Time.timeScale)

            // Caso o paciente saia da balança antes do tempo acabar
            if (w < detectionThreshold * 0.5f)
            {
                Debug.Log("Paciente saiu da balança. Medição cancelada.");
                isMeasuring = false;// Reseta flag
                yield break;// Interrompe a corrotina
            }

            yield return null;// Espera o próximo frame antes de continuar
        }

        // Calcula o peso médio durante o período
        patientWeight = (samples > 0) ? (sum / samples) : 0f;

        weightSaved = true; // Marca que o peso foi salvo
        isMeasuring = false; // Marca que não está mais medindo

        Debug.Log($"Peso armazenado: {patientWeight:F2} kg");

    }
    // Função auxiliar para resetar e poder medir de novo
    public void ResetSavedWeight()
    {
        weightSaved = false;  // Libera para medir de novo
        patientWeight = 0f;   // Zera valor salvo
    }
}