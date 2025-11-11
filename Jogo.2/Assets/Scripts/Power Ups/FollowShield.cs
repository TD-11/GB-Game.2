using UnityEngine;

public class FollowShield : MonoBehaviour
{
    public Transform player;// Referência ao transform do jogador
    public Vector3 offset = new Vector3(0, 0, 0);// Posição relativa ao jogador
    
    void Update()
    {
        if (player != null)
        {
            // Faz com que o escudo siga o player
            transform.position = player.position + offset;
        }
    } 
    // Para destruir o escudo depois de colidido com o obstáculo:
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            gameObject.SetActive(false);// Desativa o escudo
            ObjectPool.Instance.ReturnToPool("Obstacle", collision.gameObject);// Destrói o obstaculo depois de colidido
        }
    }
}
