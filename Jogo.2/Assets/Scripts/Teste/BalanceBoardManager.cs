using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BalanceBoardManager : MonoBehaviour
{
    [Header("Referências visuais (opcional)")]
    public Transform topLeft;
    public Transform topRight;
    public Transform bottomLeft;
    public Transform bottomRight;
    public RectTransform centerMarker;

    [Header("UI (opcional)")]
    public TMP_Text debugText;

    [Header("Configuração")]
    public int remoteIndex = 0; // índice do Wii Remote conectado à Balance Board

    void Update()
    {
        if (!Wii.IsActive(remoteIndex))
            return;

        // Verifica se o acessório é a Balance Board
        if (Wii.GetExpType(remoteIndex) == 3)
        {
            // Leitura dos sensores
            Vector4 sensors = Wii.GetBalanceBoard(remoteIndex);
            float totalWeight = Wii.GetTotalWeight(remoteIndex);
            Vector2 center = Wii.GetCenterOfBalance(remoteIndex);

            // Atualiza visualmente os 4 sensores (altura proporcional ao peso)
            if (topLeft)     topLeft.localScale     = new Vector3(1, 1f - (0.01f * sensors.y), 1);
            if (topRight)    topRight.localScale    = new Vector3(1, 1f - (0.01f * sensors.x), 1);
            if (bottomLeft)  bottomLeft.localScale  = new Vector3(1, 1f - (0.01f * sensors.w), 1);
            if (bottomRight) bottomRight.localScale = new Vector3(1, 1f - (0.01f * sensors.z), 1);

            // Atualiza posição do marcador do centro de gravidade
            if (centerMarker)
            {
                centerMarker.anchoredPosition = new Vector2(
                    center.x * (Screen.width / 2f),
                    center.y * (Screen.height / 2f)
                );
            }

            // Exibe informações de depuração
            if (debugText)
            {
                debugText.text =
                    $"Balance Board\n" +
                    $"Total Weight: {totalWeight:F2} kg\n" +
                    $"TopRight: {sensors.x:F2} kg\n" +
                    $"TopLeft: {sensors.y:F2} kg\n" +
                    $"BottomRight: {sensors.z:F2} kg\n" +
                    $"BottomLeft: {sensors.w:F2} kg\n" +
                    $"Center: {center}";
            }
        }
    }
}
