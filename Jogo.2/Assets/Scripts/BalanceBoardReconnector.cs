using UnityEngine;
using TMPro;
using System.Collections;

public class BalanceBoardReconnectButton : MonoBehaviour
{
    [Header("Configuração do Wii")]
    public int remoteIndex = 0;

    [Header("UI de Status")]
    public TMP_Text statusText;
    public GameObject reconnectButton;
    public GameObject manualModePanel;

    private bool isTryingToConnect = false;

    public void TryReconnectBalanceBoard()
    {
        if (!isTryingToConnect)
            StartCoroutine(ReconnectRoutine());
    }

    private IEnumerator ReconnectRoutine()
    {
        isTryingToConnect = true;
        reconnectButton.SetActive(false);
        statusText.text = "🔄 Tentando reconectar Balance Board...";
        Debug.Log("🔄 Iniciando tentativa de reconexão...");

        // 1️⃣ Encerra qualquer busca antiga e desconecta
        Wii.StopSearch();
        yield return new WaitForSeconds(0.5f);

        Wii.DropWiiRemote(remoteIndex);
        yield return new WaitForSeconds(0.5f);

        Wii.DropWiiRemote(remoteIndex);
        yield return new WaitForSeconds(0.5f);

        // 2️⃣ Reacorda a lib nativa
        if (!Wii.GetIsAwake())
        {
            Wii.GetIsAwake();
            Debug.Log("💡 Biblioteca Wii reativada.");
        }

        // 3️⃣ Inicia uma nova busca de dispositivos Bluetooth
        Wii.findWiiRemote();
        yield return new WaitForSeconds(2.5f);

        // 4️⃣ Verifica se reconectou
        if (Wii.IsActive(remoteIndex) && Wii.GetExpType(remoteIndex) == 3)
        {
            statusText.text = "✅ Balance Board reconectada!";
            manualModePanel.SetActive(false);
            reconnectButton.SetActive(false);
            Debug.Log("✅ Balance Board reconectada com sucesso!");
        }
        else
        {
            statusText.text = "❌ Falha ao reconectar. Tente novamente.";
            reconnectButton.SetActive(true);
            manualModePanel.SetActive(true);
            Debug.LogWarning("⚠️ Falha na reconexão — tente novamente.");
        }

        isTryingToConnect = false;
    }
}