using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
   private Rigidbody2D rig;
   private Vector2 movement;// Vector de direção
   public List<GameObject> Hearts = new List<GameObject>(3);
   public float speed = 3f;
   public int life;
   
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();// busca o rigid body 2D 
        life = Hearts.Count;
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
        // Caso seja um objeto ruim:
        if (collision.gameObject.tag == "Danger")
        {
            life--;
            Hearts[life].SetActive(false);
            Destroy(collision.gameObject);// Destrói o objeto depois de colidido
            
            if (life == 0)
            {
                Destroy(gameObject);
            }
        }
        // Caso seja um objeto bom:    
        if (collision.gameObject.CompareTag("Life"))
        {
            if (life == 3)
            {
                life = 3;
                Destroy(collision.gameObject);
                Debug.Log($"Vida recuperada: {life}");
            }

            if (life < 3)
            {
                life++;
                Hearts[life - 1].SetActive(true);
                Destroy(collision.gameObject);// Destrói o objeto depois de colidido 
            }
        }
    }
}
