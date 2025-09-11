using UnityEngine;

public class Destroyer : MonoBehaviour
{
    public string tagObject;
    public string tagAlertObject;
    
    // Para desativar os avisos e os objetos:
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(tagAlertObject))
        {
            ObjectPool.Instance.ReturnToPool(tagAlertObject, collision.gameObject);// Devolve para a "pool"
        }
        
        if (collision.gameObject.CompareTag("End"))
        {
            ObjectPool.Instance.ReturnToPool(tagObject, gameObject);// Devolve para a "pool"
        }
    }
}
