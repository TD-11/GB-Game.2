using System.Collections;
using TMPro;
using UnityEngine;

public class Calibragem : MonoBehaviour
{
    [Header("Configuração")]
    public static int remoteIndex = 0;// Indice do Wii Remote conectado à Balance Board
    
    public TMP_Text textPeso;
    public TMP_Text tempoCalibragem;
    private float pesoPaciente;
    private float totalWeight;
    private IEnumerator coroutine; 
    
    private bool measuring = false;   // controla se já iniciou a contagem
    private float patientWeight = 0f; // peso final armazenado
    
    void Start()
    {
        if (!Wii.IsActive(remoteIndex))
        {
            return;
        }
        
        print("Iniciando " + Time.time );

        coroutine = MeasureWeightRoutine(1);
        StartCoroutine(coroutine); 
    }

    // Update is called once per frame
    void Update()
    {
        if (Wii.GetExpType(remoteIndex) == 3)
        {
            totalWeight = Wii.GetTotalWeight(remoteIndex);
            
            if (totalWeight > 0f && totalWeight < 3.5f)
            {
                totalWeight = 0f;
            }

            if (totalWeight > 4f)
            {
                StartCoroutine(MeasureWeightRoutine(totalWeight));
            }
            textPeso.text = $"Peso do paciente: {totalWeight:F2} kg";
            Debug.Log($"Peso do paciente: {totalWeight:F2} kg");
        }
    }
    
    private IEnumerator MeasureWeightRoutine(float firstValue)
    {
        measuring = true;
        Debug.Log("Paciente detectado. Medindo peso em 5 segundos...");

        // Espera 5 segundos
        yield return new WaitForSeconds(5f);

        // Pega o peso atualizado dos sensores
        float finalWeight = totalWeight;

        // Salva o peso final
        patientWeight = finalWeight;

        Debug.Log("Peso armazenado: " + patientWeight + " kg");
    }
    
}
