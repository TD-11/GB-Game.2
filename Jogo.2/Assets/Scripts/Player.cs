using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
   private Rigidbody2D rigidbody2D;
   private Vector2 movement;
   
   public List<GameObject> Hearts = new List<GameObject>(3);// Armazenas as imagens de coração

   public FollowShield shield;
   public GameObject gameOverScreen;
   
   public float speed;
   private int life;
   private int countShell = 0;
   private int countObstacle = 0;
   private int countLife = 0;
   private int countShield = 0;
   private int initialvalue = 0;

   
   public TMP_Text countShellText;// Texto que mostra a quantidade de conchas
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        life = Hearts.Count;// Determina a quantidade de vidas conforme a quantidade de corações
    }
    void Update()
    {
        KeyboardMove();// Controle do jogo a partir do teclado
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
            life--;
            countObstacle += 1;
            Hearts[life].SetActive(false);// Apaga os corações quando levar dano
            ObjectPool.Instance.ReturnToPool("Obstacle", collision.gameObject);// Destrói o objeto depois de colidido
            
            // Quando o player morrer:
            if (life == 0)
            {
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

    void KeyboardMove()
    {
        if (Input.GetKey(KeyCode.A))
        {
            movement = new Vector2(-1, 0).normalized;// Direção
        }
        else if (Input.GetKey(KeyCode.D))
        {
            movement = new Vector2(1, 0).normalized;// Direção
        }
        else
        {
            movement = Vector2.zero;// Não se move
        }
    }

    void RestartData()
    {
        countShell = initialvalue;
        countObstacle = initialvalue;
        countLife = initialvalue;
        countShield = initialvalue;

        for (int i = 0; i <= 2; i++)
        {
            Hearts[i].SetActive(true);
        }
    }
}