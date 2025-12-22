using UnityEngine;
using TMPro;
using System.Collections;
using System.Reflection;

public class BalanceBoardReconnector : MonoBehaviour
{
    // =========================
    //      REFERÊNCIAS DE UI
    // =========================
    [Header("Referências de UI")]
    public TMP_Text statusText;        // Texto que informa o status da reconexão
    public GameObject reconnectButton; // Botão para tentar reconectar
    public GameObject manualModePanel; // Painel exibido quando entra em modo manual

    // =========================
    //  CONFIGURAÇÃO DA BALANÇA
    // =========================
    [Header("Configuração da Balance Board")]
    public int remoteIndex = 0;        // Índice do Wii Remote associado à Balance Board

    // Indica se uma tentativa de reconexão já está em andamento
    private bool isTrying = false;

    // =========================
    //     BOTÃO DE RECONEXÃO
    // =========================
    // Método chamado pelo botão "Reconectar"
    public void ReconnectButton()
    {
        // Evita múltiplas corrotinas simultâneas
        if (!isTrying)
            StartCoroutine(ReconnectRoutine());
    }

    // =========================
    //   VERIFICAÇÃO POR REFLEXÃO
    // =========================
    // Confere se um método existe na DLL do Wii
    // Isso evita crashes caso a versão da DLL não possua o método
    private bool HasMethod(string methodName)
    {
        var m = typeof(Wii).GetMethod(
            methodName,
            BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic
        );
        return m != null;
    }

    // =========================
    //   ROTINA DE RECONEXÃO
    // =========================
    // Tenta reconectar a Balance Board em até 3 tentativas
    private IEnumerator ReconnectRoutine()
    {
        isTrying = true;

        // Desativa o botão durante a tentativa
        reconnectButton.SetActive(false);

        statusText.text = "🔄 Tentando reconectar Balance Board...";
        Debug.Log("Iniciando rotina de reconexão...");

        bool reconectou = false;

        // Realiza até 3 tentativas
        for (int tentativa = 1; tentativa <= 3; tentativa++)
        {
            Debug.Log($"Tentativa {tentativa} de reconexão...");

            // 1️⃣ Interrompe buscas antigas, se disponível
            if (HasMethod("StopSearch"))
            {
                try
                {
                    Wii.StopSearch();
                    Debug.Log("StopSearch chamado.");
                }
                catch { }
            }

            yield return new WaitForSeconds(0.5f);

            // 2️⃣ Libera conexões antigas do Wii Remote
            if (HasMethod("DropWiiRemote"))
            {
                try
                {
                    Wii.DropWiiRemote(remoteIndex);
                    Debug.Log("DropWiiRemote chamado.");
                }
                catch { }
            }

            yield return new WaitForSeconds(0.5f);

            // 3️⃣ Reativa o sistema, se existir
            if (HasMethod("WakeUp"))
            {
                try
                {
                    Wii.WakeUp();
                    Debug.Log("WakeUp chamado.");
                }
                catch { }
            }

            yield return new WaitForSeconds(0.5f);

            // 4️⃣ Inicia a busca pela Balance Board
            bool iniciouBusca = false;

            if (HasMethod("Find"))
            {
                try
                {
                    Wii.findWiiRemote();
                    iniciouBusca = true;
                    Debug.Log("Find chamado.");
                }
                catch { }
            }
            else if (HasMethod("StartSearch"))
            {
                try
                {
                    Wii.StartSearch();
                    iniciouBusca = true;
                    Debug.Log("StartSearch chamado.");
                }
                catch { }
            }

            // Aguarda o tempo necessário para a busca
            if (iniciouBusca)
                yield return new WaitForSeconds(2.5f);
            else
                yield return new WaitForSeconds(1f);

            // 5️⃣ Verifica se a reconexão foi bem-sucedida
            try
            {
                if (Wii.IsActive(remoteIndex) && Wii.GetExpType(remoteIndex) == 3)
                {
                    reconectou = true;
                    break;
                }
            }
            catch { }

            Debug.LogWarning($"Tentativa {tentativa} falhou. Tentando novamente...");
        }

        // =========================
        //      RESULTADO FINAL
        // =========================
        if (reconectou)
        {
            // Reconexão bem-sucedida
            statusText.text = "✅ Balance Board reconectada!";
            manualModePanel.SetActive(false);
            reconnectButton.SetActive(false);

            Debug.Log("Balance Board reconectada com sucesso!");
        }
        else
        {
            // Falha após todas as tentativas
            statusText.text = "❌ Falha na reconexão. Modo manual ativado.";
            manualModePanel.SetActive(true);
            reconnectButton.SetActive(true);

            Debug.LogWarning("Falha final na reconexão. Entrando em modo manual.");
        }

        isTrying = false;
    }
}