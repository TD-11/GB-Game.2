using System;
using System.Threading;
using UnityEditor.VersionControl;
using UnityEngine;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

public class FollowShield : MonoBehaviour
{
    // Referência ao Transform do jogador que o escudo deve seguir
    public Transform player;

    // Offset para ajustar a posição do escudo em relação ao jogador
    public Vector3 offset = new Vector3(0, 0, 0);

    private AudioSource playerAudio;
    [SerializeField]
    private AudioClip shieldDestroyedSound;
    
    Animator animator;
    
    private void Start()
    {
        playerAudio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Verifica se a referência ao jogador foi atribuída
        if (player != null)
        {
            // Atualiza a posição do escudo para ficar sempre na posição do jogador + deslocamento
            // Isso faz o escudo seguir o jogador em tempo real
            transform.position = player.position + offset;
        }
    } 
    
    // Detecta colisões com objetos marcados como "Obstacle"
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Se o objeto que colidiu tiver a tag "Obstacle"
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            playerAudio.PlayOneShot(shieldDestroyedSound, 2f); // Som do escudo destruído

            // Desativa o escudo após a colisão (como se tivesse sido gasto)
            //await Task.Delay(2000);
            //gameObject.SetActive(false);
            animator.SetTrigger("Dano");

            // Retorna o obstáculo colidido para o Object Pool
            // Isso evita destruí-lo e recriá-lo, economizando desempenho
            ObjectPool.Instance.ReturnToPool("Obstacle", collision.gameObject);
        }
    }

    public void PlaySFX()
    {
        playerAudio.Play();
    }

    public void Desligar()
    {
        gameObject.SetActive(false);
    }
}