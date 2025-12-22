using UnityEngine;

public class ErrorManualModeText : MonoBehaviour
{
    // =========================
    //  CONFIGURAÇÃO DA BALANÇA
    // =========================
    [Header("Configuração da Balança")]
    public static int remoteIndex = 0; // Índice do Wii Remote associado à Balance Board

    // =========================
    //      REFERÊNCIAS DE UI
    // =========================
    public GameObject manualModeText; // Texto/painel exibido quando o modo manual está ativo

    // Cores configuráveis (atualmente não utilizadas diretamente neste script,
    // mas podem ser aproveitadas futuramente para feedback visual)
    public Color normalColor = Color.green; // Cor normal (balança conectada)
    public Color alertColor = Color.red;    // Cor de alerta (balança desconectada)

    // =========================
    //   REFERÊNCIA ESTÁTICA
    // =========================
    public GameObject errorTextInstance; // Objeto atribuído via Inspector
    public static GameObject errorText;  // Referência global acessível por outros scripts

    // =========================
    //      INICIALIZAÇÃO
    // =========================
    void Awake()
    {
        // Armazena a instância do texto de erro em uma variável estática,
        // permitindo acesso global sem precisar de referência direta
        errorText = errorTextInstance;
    }

    // =========================
    //     CICLO DE ATUALIZAÇÃO
    // =========================
    void Update()
    {
        // Verifica continuamente se o modo manual deve ser exibido
        CheckManualMode();
    }

    // =========================
    //    VERIFICAÇÃO DO MODO
    // =========================
    public void CheckManualMode()
    {
        // Verifica se existe um Wii Remote ativo
        if (Wii.IsActive(remoteIndex))
        {
            // Caso o acessório conectado seja uma Balance Board (ExpType == 3),
            // o modo manual é desativado
            if (Wii.GetExpType(remoteIndex) == 3)
            {
                manualModeText.SetActive(false);
            }
        }
        else
        {
            // Se não houver Wii Remote ativo,
            // o modo manual é ativado automaticamente
            manualModeText.SetActive(true);
        }
    }
}