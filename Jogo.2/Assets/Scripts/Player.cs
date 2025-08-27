using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
   private Rigidbody2D rig;
   public FollowShield shield;// Cria um objeto para acessar a classe "FollowShield"
   private Vector2 movement;
   public GameObject gameOverScreen;
   public List<GameObject> Hearts = new List<GameObject>(3);// Armazenas as imagens de coração
   public float speed = 3f;
   public int life;
   
   public TMP_Text countShellText;// Texto que mostra a quantidade de conchas
   public TMP_Text scoreText;// Texto que mostra os pontos totais
   public TMP_Text restTimeText;// Texto que mostra o tempo que sobrou
   
   // Esse texto armazena o valor do "Time" na hierarquia
   // para igualar os valores com a interface
   public TMP_Text timeText;
   public int countShell = 0;// Conta as conchas
   public int score = 0;// Conta os pontos totais
   
   
   
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();// Busca o rigid body 2D 
        life = Hearts.Count;// Determina a quantidade de vidas conforme a quantidade de corações
    }
    
    void Update()
    {
        // Sistema de controle
        if (Input.GetKey(KeyCode.A))
        {
            movement = new Vector2(-1, 0).normalized;// Direção
        }
        else if (Input.GetKey(KeyCode.D))
        {
            movement = new Vector2(1, 0).normalized; // Direção
        }
        else
        {
            movement = Vector2.zero;// Não se move
        }
    }
    
    void FixedUpdate()
    {
        rig.linearVelocity = movement * speed;// Aplica  a velocidade ao movimento
    }

    //Identifica que tipo de objeto está colidindo com o personagem
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Caso seja um obstaculo:
        if (collision.gameObject.tag == "Danger")
        {
            life--;
            score -= 10;
            scoreText.text = "Pontos totais: " + score.ToString();// Salva o contador "score" na interface
            Hearts[life].SetActive(false);// Apaga os corações quando levar dano
            ObjectPool.Instance.ReturnToPool("Danger", collision.gameObject);// Destrói o objeto depois de colidido
            
            // Quando o player perder todas as vidas
            // Ele irá ser destruído e aparecerá a tela de "game over"
            if (life == 0)
            {
                ObjectPool.Instance.ReturnToPool("Shield", gameObject);
                Time.timeScale = 0f;// Pausa o tempo
                restTimeText.text = "Tempo restante: " + timeText.text;// Iguala o tempo restante na interface
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
                scoreText.text = "Pontos totais: " + score.ToString();// Salva o contador "score" na interface
                ObjectPool.Instance.ReturnToPool("Life", collision.gameObject);// Destrói o objeto depois de colidido 
                Debug.Log($"Vida recuperada: {life}");
            }
            // Caso a quantidade de vidas for menor que 3, ele recuperará vida
            if (life < 3)
            {
                life++;
                score += 20; 
                scoreText.text = "Pontos totais: " + score.ToString();// Salva o contador "score" na interface
                Hearts[life - 1].SetActive(true);// Reativa as imagens de coração
                ObjectPool.Instance.ReturnToPool("Life", collision.gameObject);// Destrói o objeto depois de colidido 
            }
        }
        
        if (collision.gameObject.CompareTag("Shell"))
        {
            countShell += 10;
            score += 10;
            countShellText.text = countShell.ToString();// Mostra a quantidade de pontos de concha no HUD
            scoreText.text = "Pontos totais: " +  score.ToString();// Salva o contador "score" na interface
            ObjectPool.Instance.ReturnToPool("Shell", collision.gameObject);// Destrói o objeto depois de colidido 
        }

        if (collision.gameObject.CompareTag("Shield"))
        {
            score += 75;
            scoreText.text = "Pontos totais: " + score.ToString();// Salva o contador "score" na interface
            shield.gameObject.SetActive(true);// Ativa o poder do escudo 
            ObjectPool.Instance.ReturnToPool("Shield", collision.gameObject);// Destrói o objeto depois de colidido 
        }
    }
}