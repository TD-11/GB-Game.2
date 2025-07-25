using System;
using UnityEditor.VersionControl;
using UnityEngine;

public class DestroyObjects : MonoBehaviour
{
    // Para desativar os objetos que vão cair
    public float timeDestroy;
    void Update()
    {
        Destroy(gameObject, timeDestroy);
    }
    // Para desativar os avisos:
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("LifeAlert"))
        {
            ObjectPool.Instance.ReturnToPool("LifeAlert", collision.gameObject);
        }
        
        if (collision.gameObject.CompareTag("ObstacleAlert"))
        {
            ObjectPool.Instance.ReturnToPool("ObstacleAlert", collision.gameObject);
        }
        
        if (collision.gameObject.CompareTag("ShellAlert"))
        {
            ObjectPool.Instance.ReturnToPool("ShellAlert", collision.gameObject);
        }
        
        if (collision.gameObject.CompareTag("ShieldAlert"))
        {
            ObjectPool.Instance.ReturnToPool("ShieldAlert", collision.gameObject);
        }
        
    }
}
