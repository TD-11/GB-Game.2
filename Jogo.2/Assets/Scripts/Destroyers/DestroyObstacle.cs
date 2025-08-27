using System;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;

public class DestroyObstacle : MonoBehaviour
{
    
    // Para desativar os avisos e os objetos:
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ObstacleAlert"))
        {
            ObjectPool.Instance.ReturnToPool("ObstacleAlert", collision.gameObject);// Devolve para a "pool"
        }
        
        if (collision.gameObject.CompareTag("End"))
        {
            ObjectPool.Instance.ReturnToPool("Danger", gameObject);// Devolve para a "pool"
        }
    }
}
