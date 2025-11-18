using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
   private Rigidbody2D rigidbody2D;
   private Vector2 movement;
   private bool facingRight = true; // Controle de direção atual

   public GameObject camera;

   
   public List<GameObject> Hearts = new List<GameObject>(3);// Armazenas as imagens de coração
   [SerializeField]
   public FollowShield shield;
   [SerializeField]
   public GameObject gameOverScreen;
   public GameObject fallConnectionScreen;
   
   public float speed;
   private int life;
   private int countShell = 0;
   private int countObstacle = 0;
   private int countLife = 0;
   private int countShield = 0;
   private bool manualMode;
   private bool lastConnectionState = false; // guarda o estado anterior da balança
   
   //private int initialvalue = 0;
   

   [SerializeField]
   public TMP_Text countShellText;// Texto que mostra a quantidade de conchas
   
   // Relacionados a Wii Board
   [Header("Configuração")]
   public static int remoteIndex = 0;// Indice do Wii Remote conectado à Balance Board
   //=========================
   
   // Relacionados a SD Balance
   public SD_SerialManager _sd_serialManager;
   public float renge = 10000;
   public float PesoCalibrado = 0;
   
   public float Esquerda = 0;
   public float Direita = 0;
   //=========================


   
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        life = Hearts.Count;// Determina a quantidade de vidas conforme a quantidade de corações
        
        // Já define o modo correto ao iniciar o jogo
        bool isBoardConnected = Wii.IsActive(remoteIndex) && Wii.GetExpType(remoteIndex) == 3;
        manualMode = !isBoardConnected;
        //fallConnectionScreen.SetActive(!isBoardConnected);
    }
    void Update()
    {
        /*
        bool isBoardConnected = Wii.IsActive(remoteIndex) && Wii.GetExpType(remoteIndex) == 3;

        // Detecta mudança de conexão (ex: desconectou ou reconectou)
        if (isBoardConnected != lastConnectionState)
        {
            if (isBoardConnected)
            {
                // Reconectou
                manualMode = false;
                fallConnectionScreen.SetActive(false);
                Time.timeScale = 1f;// Pausa o tempo

                Debug.Log("Balance Board conectada! Jogo normalizado!");
            }
            else
            {
                // Desconectou
                manualMode = true;
                fallConnectionScreen.SetActive(true);
                Time.timeScale = 0f;// Pausa o tempo

                Debug.LogWarning("Balance Board desconectada! Modo manual ativado.");
            }

            lastConnectionState = isBoardConnected; // atualiza estado
        }
        */
        // Define o controle de movimento com base no modo atual
        
        if (SD_SerialManager.Instance != null && SD_SerialManager.Instance.A > 0f) {
            float valor = SD_SerialManager.Instance.A;
        }
        
        if (manualMode)
        {
            KeyboardMove();
        }
        else
        {
            SDBalanceMove();
            //NintendoBalanceBoardMove();
        }
    }
    void FixedUpdate()
    {
        rigidbody2D.linearVelocity = movement * speed;// Aplica  a velocidade ao movimento
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Caso seja um obstaculo:
        if (collision.gameObject.tag == "Obstacle")
        {
            camera.GetComponent<TREMOR>().playTremor();
            
            life--;
            countObstacle += 1;
            Hearts[life].SetActive(false);// Apaga os corações quando levar dano
            ObjectPool.Instance.ReturnToPool("Obstacle", collision.gameObject);// Destrói o objeto depois de colidido
            
            // Quando o player morrer:
            if (life == 0)
            {
                Destroy(gameObject);
                Time.timeScale = 0f;// Pausa o tempo
                gameOverScreen.SetActive(true);
            }
            
        }
        // Caso seja um objeto bom:    
        if (collision.gameObject.CompareTag("Life"))
        {
            //Quando se tem 3 vidas, a quantidade não vai mais se modificar
            if (life == 3)
            {
                life = 3;
                ObjectPool.Instance.ReturnToPool("Life", collision.gameObject);// Destrói o objeto depois de colidido 
                Debug.Log($"Vida recuperada: {life}");
            }
            // Caso a quantidade de vidas for menor que 3, ele recuperará vida
            if (life < 3)
            {
                life++;
                countLife += 1; 
                Hearts[life - 1].SetActive(true);// Reativa as imagens de coração
                ObjectPool.Instance.ReturnToPool("Life", collision.gameObject);// Destrói o objeto depois de colidido 
            }
        }
        
        if (collision.gameObject.CompareTag("Shell"))
        {
            countShell += 1;
            countShellText.text = countShell.ToString();// Mostra a quantidade de pontos de concha no HUD
            ObjectPool.Instance.ReturnToPool("Shell", collision.gameObject);// Destrói o objeto depois de colidido 
        }

        if (collision.gameObject.CompareTag("Shield"))
        {
            countShield += 1;
            shield.gameObject.SetActive(true);// Ativa o poder do escudo 
            ObjectPool.Instance.ReturnToPool("Shield", collision.gameObject);// Destrói o objeto depois de colidido 
        }
    }
    void Flip()
    {
        facingRight = !facingRight; // troca o estado (direita/esquerda)
        Vector3 scale = transform.localScale;
        scale.x *= -1; // multiplica por -1 para inverter a direção
        transform.localScale = scale;
    }
    // Controle do jogo a partir do teclado
    void KeyboardMove()
    {
        if (Input.GetKey(KeyCode.A))
        {
            movement = new Vector2(-1, 0).normalized;// Direção
            if (facingRight)
            {
                Flip(); // Vira para a esquerda
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            movement = new Vector2(1, 0).normalized;// Direção
            if (!facingRight)
            {
                Flip(); // Vira para a esquerda
            }
        }
        else
        {
            movement = Vector2.zero;// Não se move
        }
    }

    void SDBalanceMove()
    {
        // deve ser adicionada uma rotina de calibração do pesso do paciente par aservir como referencia
        // da zona morta que não ativarar a movimetacão.
        // teste de movimentação sem calibração
        if (_sd_serialManager != null)
        {
            PesoCalibrado = _sd_serialManager.P;
            Esquerda = (_sd_serialManager.A + _sd_serialManager.C);
            Debug.Log("AC..." + Esquerda);
            
            Direita = (_sd_serialManager.B + _sd_serialManager.D);
            Debug.Log("BD..." + Direita);
            
            if (Esquerda >  (PesoCalibrado/2 + renge) )
            {
                transform.position += Vector3.left * speed * Time.deltaTime;
            }
              
            if ( Direita > (PesoCalibrado/2 + renge) )
            {
                transform.position += Vector3.right * speed * Time.deltaTime;
            }
        }
    }
    
    // Controle do jogo a partir do Wii Board
    void NintendoBalanceBoardMove()
    {

        if (!Wii.IsActive(remoteIndex))
        {
            return;
        }

        // Verifica se o acessório é a Balance Board
        if (Wii.GetExpType(remoteIndex) == 3)
        {
            // Leitura dos sensores
            Vector4 sensors = Wii.GetBalanceBoard(remoteIndex);
            //Vector2 center = Wii.GetCenterOfBalance(remoteIndex);// Para definir o centro de gravidade
            
            // Prevenção de ruído
            if (sensors.x > 0f && sensors.x < 1.3f)
            {
                sensors.x = 0f;
            }
            else if (sensors.y > -1f && sensors.y < 0f)
            {
                sensors.y = 0f;
            }
            else if (sensors.w > -1f && sensors.w < 0f)
            {
                sensors.w = 0f;
            }
            else if (sensors.z > 0 && sensors.z < 2.90f)
            {
                sensors.z = 0f;
            }

            if ((sensors.y + sensors.w)  > (BalanceBoardCalibration.playerWeight / 2) + 5 )
            {
                movement = new Vector2(-1, 0);// Direção
                if (facingRight)
                {
                    Flip(); // Vira para a esquerda
                }
            }
            
            else if (sensors.x + sensors.z > (BalanceBoardCalibration.playerWeight / 2) + 5 )
            {
                movement = new Vector2(1, 0);// Direção
                if (!facingRight)
                {
                    Flip(); // Vira para a esquerda
                }
            }
            
            else
            {
                movement = Vector2.zero;// Não se move
            }
            
            // Exibe os valores do sensores
            Debug.Log($"Quadrante 1: {sensors.x:F2} kg;" + $"Quadrante 2: {sensors.y:F2} kg;" + $"Quadrante 3: {sensors.w:F2} kg;" + $"Quadrante 4: {sensors.z:F2} kg");
        }
    }
    
}