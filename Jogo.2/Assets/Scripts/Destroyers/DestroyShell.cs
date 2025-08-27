using System;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;

public class DestroyShell : MonoBehaviour
{
    // Para desativar os avisos e os objetos:
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ShellAlert"))
        {
            ObjectPool.Instance.ReturnToPool("ShellAlert", collision.gameObject);// Devolve para a "pool"
        }
        
        if (collision.gameObject.CompareTag("End"))
        {
            ObjectPool.Instance.ReturnToPool("Shell", gameObject);// Devolve para a "pool"
        }
    }
}
