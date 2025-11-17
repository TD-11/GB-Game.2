using UnityEngine;

public class Destroyer : MonoBehaviour
{
    // Tag do objeto principal que será retornado à pool quando atingir a área marcada com "End"
    public string tagObject;

    // Tag do objeto de alerta (ex.: aviso visual, indicador) que também será retornado à pool
    public string tagAlertObject;
    
    // Este método é chamado automaticamente pelo Unity quando outro collider 2D entra no
    // trigger deste objeto (ou seja, atravessa sua área de colisão marcada como Trigger).
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se o objeto que entrou no trigger possui a tag de alerta.
        // Caso possua, devolve esse objeto para a pool e o desativa.
        if (collision.gameObject.CompareTag(tagAlertObject))
        {
            // Retorna o objeto de alerta para o Object Pool, economizando processamento
            // ao invés de destruir e recriar objetos.
            ObjectPool.Instance.ReturnToPool(tagAlertObject, collision.gameObject);
        }
        
        // Verifica se o objeto que colidiu possui a tag "End".
        // Geralmente isso significa que um objeto passou do limite da tela ou chegou ao destino final.
        if (collision.gameObject.CompareTag("End"))
        {
            // Retorna este próprio objeto (gameObject) para a pool,
            // usando sua tag configurada no inspetor.
            ObjectPool.Instance.ReturnToPool(tagObject, gameObject);
        }
    }
}

