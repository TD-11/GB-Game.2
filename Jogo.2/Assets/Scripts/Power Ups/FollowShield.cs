using UnityEngine;

public class FollowShield : MonoBehaviour
{
    // Referência ao Transform do jogador que o escudo deve seguir
    public Transform player;

    // Offset para ajustar a posição do escudo em relação ao jogador
    public Vector3 offset = new Vector3(0, 0, 0);
    
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
            // Desativa o escudo após a colisão (como se tivesse sido gasto)
            gameObject.SetActive(false);

            // Retorna o obstáculo colidido para o Object Pool
            // Isso evita destruí-lo e recriá-lo, economizando desempenho
            ObjectPool.Instance.ReturnToPool("Obstacle", collision.gameObject);
        }
    }
}