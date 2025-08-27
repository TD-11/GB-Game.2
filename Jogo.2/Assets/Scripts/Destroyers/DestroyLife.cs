using System;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;

public class DestroyLife : MonoBehaviour
{
    // Para desativar os avisos e os objetos:
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("LifeAlert"))
        {
            ObjectPool.Instance.ReturnToPool("LifeAlert", collision.gameObject);// Devolve para a "pool"
        }
        
        if (collision.gameObject.CompareTag("End"))
        {
            ObjectPool.Instance.ReturnToPool("Life", gameObject);// Devolve para a "pool"
        }
    }
}
