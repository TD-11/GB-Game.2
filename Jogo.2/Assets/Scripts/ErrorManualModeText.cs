using UnityEngine;

public class ErrorManualModeText : MonoBehaviour
{
    [Header("Configuração da Balança")]
    public static int remoteIndex = 0; // Índice do Wii Remote conectado
    
    public GameObject manualModeText;

    public Color normalColor = Color.green;// Define a cor normal do aviso
    public Color alertColor = Color.red;// Define a cor de aviso do aviso
    
    public GameObject errorTextInstance;
    public static GameObject errorText;
    
    void Awake()
    {
        errorText = errorTextInstance;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckManualMode();
    }
    public void CheckManualMode()
    {
        if(Wii.IsActive(remoteIndex))
        {
            // Verifica se o acessório conectado é uma Balance Board (tipo 3)
            if (Wii.GetExpType(remoteIndex) == 3)
            {
                manualModeText.SetActive(false);
            }
        }
        
        else
        {
            manualModeText.SetActive(true);
        }
    }
}
