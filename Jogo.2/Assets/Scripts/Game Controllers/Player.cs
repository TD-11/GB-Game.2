using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;      // Referência ao Rigidbody2D para aplicar movimento
    private Vector2 movement;             // Vetor de movimento horizontal
    private bool facingRight = true;      // Indica se o player está olhando para a direita

    public GameObject camera;             // Câmera usada para efeitos visuais (ex.: tremor)

    public List<GameObject> Hearts = new List<GameObject>(3); // Lista das imagens de coração da UI (vidas)
    
    [SerializeField] public FollowShield shield;      // Referência ao escudo de proteção
    [SerializeField] public GameObject gameOverScreen;// Tela de Game Over
    public GameObject fallConnectionScreen;           // Tela exibida quando a Balance Board cai/desconecta

    public float speed;                   // Velocidade do player
    private int life;                     // Quantidade de vidas
    private int countShell = 0;           // Pontuação de conchas coletadas
    private int countObstacle = 0;        // Quantidade de obstáculos atingidos
    private int countLife = 0;            // Quantidade de vidas coletadas
    private int countShield = 0;          // Quantidade de escudos coletados
    private bool manualMode;              // Se o jogador está controlando com teclado
    private bool lastConnectionState = false; // Guarda o estado de conexão anterior da Balance Board

    [SerializeField] public TMP_Text countShellText;// Texto que mostra quantidade de conchas no HUD

    // Configuração da Balance Board
    [Header("Configuração")]
    public static int remoteIndex = 0; // Índice do Wii Remote conectado à Balance Board

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();

        // Define vidas iniciais de acordo com o número de corações
        life = Hearts.Count;

        // Verifica se a Balance Board está conectada — se não estiver, entra em modo manual
        bool isBoardConnected = Wii.IsActive(remoteIndex) && Wii.GetExpType(remoteIndex) == 3;
        manualMode = !isBoardConnected;
    }

    void Update()
    {
        // Verifica se a Balance Board está conectada
        bool isBoardConnected = Wii.IsActive(remoteIndex) && Wii.GetExpType(remoteIndex) == 3;

        // Detecta mudança no estado de conexão para alternar modo manual/automático
        if (isBoardConnected != lastConnectionState)
        {
            if (isBoardConnected)
            {
                // Se reconectou: volta ao modo automático
                manualMode = false;
                fallConnectionScreen.SetActive(false);
                Time.timeScale = 1f;

                Debug.Log("Balance Board conectada! Jogo normalizado!");
            }
            else
            {
                // Se desconectou: ativa modo manual e pausa o jogo
                manualMode = true;
                fallConnectionScreen.SetActive(true);
                Time.timeScale = 0f;

                Debug.LogWarning("Balance Board desconectada! Modo manual ativado.");
            }

            lastConnectionState = isBoardConnected;
        }

        // Controle de movimento dependendo do modo
        if (manualMode)
            KeyboardMove();
        else
            NintendoBalanceBoardMove();
    }

    void FixedUpdate()
    {
        // Aplica o movimento no Rigidbody2D
        rigidbody2D.linearVelocity = movement * speed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Colisão com obstáculo
        if (collision.gameObject.tag == "Obstacle")
        {
            camera.GetComponent<Tremor>().playTremor(); // Efeito de tremor
            life--;                                     // Diminui vida
            countObstacle++;

            Hearts[life].SetActive(false);              // Desativa um coração
            ObjectPool.Instance.ReturnToPool("Obstacle", collision.gameObject);

            // Se morrer (0 vidas)
            if (life == 0)
            {
                Destroy(gameObject);
                Time.timeScale = 0f;
                gameOverScreen.SetActive(true);
            }
        }

        // Colisão com item de recuperação de vida
        if (collision.gameObject.CompareTag("Life"))
        {
            if (life == 3)
            {
                // Não passa de 3 vidas
                ObjectPool.Instance.ReturnToPool("Life", collision.gameObject);
            }
            else
            {
                // Recupera vida
                life++;
                countLife++;
                Hearts[life - 1].SetActive(true);  // Reativa coração
                ObjectPool.Instance.ReturnToPool("Life", collision.gameObject);
            }
        }

        // Colisão com concha (pontuação)
        if (collision.gameObject.CompareTag("Shell"))
        {
            countShell++;
            countShellText.text = countShell.ToString();
            ObjectPool.Instance.ReturnToPool("Shell", collision.gameObject);
        }

        // Colisão com escudo
        if (collision.gameObject.CompareTag("Shield"))
        {
            countShield++;
            shield.gameObject.SetActive(true);
            ObjectPool.Instance.ReturnToPool("Shield", collision.gameObject);
        }
    }

    // Inverte a direção visual do player
    void Flip()
    {
        facingRight = !facingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1; // espelha horizontalmente
        transform.localScale = scale;
    }

    // Controle manual pelo teclado (modo fallback)
    void KeyboardMove()
    {
        if (Input.GetKey(KeyCode.A))
        {
            movement = new Vector2(-1, 0).normalized;
            if (facingRight) Flip();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            movement = new Vector2(1, 0).normalized;
            if (!facingRight) Flip();
        }
        else
        {
            movement = Vector2.zero;
        }
    }

    // Controle via Balance Board
    void NintendoBalanceBoardMove()
    {
        if (!Wii.IsActive(remoteIndex)) return;

        // Confirma se é realmente a Balance Board
        if (Wii.GetExpType(remoteIndex) == 3)
        {
            // Sensores da balança
            Vector4 sensors = Wii.GetBalanceBoard(remoteIndex);

            // Filtragem de ruído dos sensores
            if (sensors.x > 0f && sensors.x < 1.3f) sensors.x = 0f;
            else if (sensors.y > -1f && sensors.y < 0f) sensors.y = 0f;
            else if (sensors.w > -1f && sensors.w < 0f) sensors.w = 0f;
            else if (sensors.z > 0 && sensors.z < 2.90f) sensors.z = 0f;

            // Inclinação para esquerda (peso maior nos sensores y + w)
            if ((sensors.y + sensors.w) > (BalanceBoardCalibration.playerWeight / 2) + 5)
            {
                movement = new Vector2(-1, 0);
                if (facingRight) Flip();
            }
            // Inclinação para a direita (peso maior nos sensores x + z)
            else if (sensors.x + sensors.z > (BalanceBoardCalibration.playerWeight / 2) + 5)
            {
                movement = new Vector2(1, 0);
                if (!facingRight) Flip();
            }
            else
            {
                movement = Vector2.zero; // Sem movimento
            }

            // Debug dos quatro sensores da Balance Board
            Debug.Log(
                $"Quadrante 1: {sensors.x:F2} kg; " +
                $"Quadrante 2: {sensors.y:F2} kg; " +
                $"Quadrante 3: {sensors.w:F2} kg; " +
                $"Quadrante 4: {sensors.z:F2} kg"
            );
        }
    }
}