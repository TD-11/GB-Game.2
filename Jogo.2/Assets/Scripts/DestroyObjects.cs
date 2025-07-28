using System;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;

public class DestroyObjects : MonoBehaviour
{
    public string tag;
    // Para desativar os avisos eos objetos:
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(tag))
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
        
        if (collision.gameObject.CompareTag("End"))
        {
            ObjectPool.Instance.ReturnToPool("Danger", gameObject);
            ObjectPool.Instance.ReturnToPool("Life", gameObject);
            ObjectPool.Instance.ReturnToPool("Shield", gameObject);
            ObjectPool.Instance.ReturnToPool("Shell", gameObject);
        }
    }
}
