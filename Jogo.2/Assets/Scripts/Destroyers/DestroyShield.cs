using System;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;

public class DestroyShield : MonoBehaviour
{
    // Para desativar os avisos eos objetos:
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ShieldAlert"))
        {
            ObjectPool.Instance.ReturnToPool("ShieldAlert", collision.gameObject);
        }
        
        if (collision.gameObject.CompareTag("End"))
        {
            ObjectPool.Instance.ReturnToPool("Shield", gameObject);
        }
    }
}
