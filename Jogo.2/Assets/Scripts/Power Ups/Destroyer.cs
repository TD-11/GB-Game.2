using UnityEngine;

// Classe responsável por remover/desativar objetos quando eles
// atingem uma área específica (geralmente fora da tela)
public class Destroyer : MonoBehaviour
{
    // Tag do objeto principal que será devolvido ao Object Pool
    public string tagObject;

    // Tag dos objetos de aviso (alertas visuais)
    public string tagAlertObject;
    
    // Método chamado automaticamente quando outro Collider2D
    // entra na área de trigger deste objeto
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Se o objeto que entrou for um alerta
        if (collision.gameObject.CompareTag(tagAlertObject))
        {
            // Retorna o objeto de alerta para o Object Pool
            ObjectPool.Instance.ReturnToPool(tagAlertObject, collision.gameObject);
        }
        
        // Se o objeto que entrou possuir a tag "End"
        // normalmente indica que o objeto saiu da área jogável
        if (collision.gameObject.CompareTag("End"))
        {
            // Retorna o objeto principal para o Object Pool
            // (este próprio GameObject)
            ObjectPool.Instance.ReturnToPool(tagObject, gameObject);
        }
    }
}
