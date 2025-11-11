using UnityEngine;
using TMPro;
using System.Collections;
using System.Reflection;

public class BalanceBoardReconnector : MonoBehaviour
{
    [Header("Referências de UI")] public TMP_Text statusText;
    public GameObject reconnectButton;
    public GameObject manualModePanel;

    [Header("Configuração da Balance Board")]
    public int remoteIndex = 0;

    private bool isTrying = false;

    // Método chamado pelo botão "Reconectar"
    public void ReconnectButton()
    {
        if (!isTrying)
            StartCoroutine(ReconnectRoutine());
    }

    // 🔍 Verifica se o método realmente existe na DLL
    private bool HasMethod(string methodName)
    {
        var m = typeof(Wii).GetMethod(methodName, BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);
        return m != null;
    }

    // 🔁 Tentativa de reconexão com até 3 tentativas
    private IEnumerator ReconnectRoutine()
    {
        isTrying = true;
        reconnectButton.SetActive(false);
        statusText.text = "🔄 Tentando reconectar Balance Board...";
        Debug.Log("Iniciando rotina de reconexão...");

        bool reconectou = false;

        for (int tentativa = 1; tentativa <= 3; tentativa++)
        {
            Debug.Log($"Tentativa {tentativa} de reconexão...");

            // 1️⃣ Para buscas antigas
            if (HasMethod("StopSearch"))
            {
                try
                {
                    Wii.StopSearch();
                    Debug.Log("StopSearch chamado.");
                }
                catch
                {
                }
            }

            yield return new WaitForSeconds(0.5f);

            // 2️⃣ Libera possíveis conexões antigas
            if (HasMethod("DropWiiRemote"))
            {
                try
                {
                    Wii.DropWiiRemote(remoteIndex);
                    Debug.Log("DropWiiRemote chamado.");
                }
                catch
                {
                }
            }

            yield return new WaitForSeconds(0.5f);

            // 3️⃣ Acorda o sistema, se houver
            if (HasMethod("WakeUp"))
            {
                try
                {
                    Wii.WakeUp();
                    Debug.Log("WakeUp chamado.");
                }
                catch
                {
                }
            }

            yield return new WaitForSeconds(0.5f);

            // 4️⃣ Inicia busca
            bool iniciouBusca = false;
            if (HasMethod("Find"))
            {
                try
                {
                    Wii.findWiiRemote();
                    iniciouBusca = true;
                    Debug.Log("Find chamado.");
                }
                catch
                {
                }
            }
            else if (HasMethod("StartSearch"))
            {
                try
                {
                    Wii.StartSearch();
                    iniciouBusca = true;
                    Debug.Log("StartSearch chamado.");
                }
                catch
                {
                }
            }

            if (iniciouBusca)
                yield return new WaitForSeconds(2.5f);
            else
                yield return new WaitForSeconds(1f);

            // 5️⃣ Testa se reconectou
            try
            {
                if (Wii.IsActive(remoteIndex) && Wii.GetExpType(remoteIndex) == 3)
                {
                    reconectou = true;
                    break;
                }
            }
            catch
            {
            }

            Debug.LogWarning($"Tentativa {tentativa} falhou. Tentando novamente...");
        }

        // 6️⃣ Resultado final
        if (reconectou)
        {
            statusText.text = "✅ Balance Board reconectada!";
            manualModePanel.SetActive(false);
            reconnectButton.SetActive(false);
            Debug.Log("Balance Board reconectada com sucesso!");
        }
        else
        {
            statusText.text = "❌ Falha na reconexão. Modo manual ativado.";
            manualModePanel.SetActive(true);
            reconnectButton.SetActive(true);
            Debug.LogWarning("Falha final na reconexão. Entrando em modo manual.");
        }

        isTrying = false;
    }
}