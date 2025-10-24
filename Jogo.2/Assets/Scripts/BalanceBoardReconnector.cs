using TMPro;
using UnityEngine;

public class BalanceBoardReconnector : MonoBehaviour
{
     [Header("Configuração do Wii")]
    public int remoteIndex = 0;

    [Header("UI de Status")]
    public TMP_Text statusText;
    public GameObject manualModePanel;

    [Header("Controle de Reconexão")]
    public float checkInterval = 3f;
    private float nextCheckTime = 0f;

    private bool isConnected = false;

    void Start()
    {
        CheckConnection(true);
    }

    void Update()
    {
        if (Time.time >= nextCheckTime)
        {
            nextCheckTime = Time.time + checkInterval;
            CheckConnection();
        }
    }

    void CheckConnection(bool firstCheck = false)
    {
        bool currentlyConnected = Wii.IsActive(remoteIndex) && Wii.GetExpType(remoteIndex) == 3;

        // Se o estado mudou (ex: conectou ↔ desconectou)
        if (currentlyConnected != isConnected || firstCheck)
        {
            isConnected = currentlyConnected;

            if (isConnected)
            {
                OnConnected();
            }
            else
            {
                OnDisconnected();
                TryReconnect();
            }
        }
    }

    void OnConnected()
    {
        Debug.Log("✅ Balance Board conectada!");
        statusText.text = "Modo: Balance Board";
        manualModePanel.SetActive(false);
    }

    void OnDisconnected()
    {
        Debug.LogWarning("⚠️ Balance Board desconectada! Tentando reconectar...");
        statusText.text = "⚠️ Balance Board desconectada\nAtivando modo manual...";
        manualModePanel.SetActive(true);
    }

    void TryReconnect()
    {
        // Desconecta qualquer conexão anterior e tenta nova busca
        Wii.DropWiiRemote(remoteIndex);
        Wii.StartSearch();

        // Aguarda 2 segundos antes de verificar de novo
        Invoke(nameof(RecheckAfterAttempt), 2f);
    }

    void RecheckAfterAttempt()
    {
        if (Wii.IsActive(remoteIndex) && Wii.GetExpType(remoteIndex) == 3)
        {
            Debug.Log("✅ Reconectado com sucesso!");
            OnConnected();
        }
        else
        {
            Debug.LogWarning("❌ Falha na reconexão. Tentando novamente em alguns segundos...");
        }
    }
}
