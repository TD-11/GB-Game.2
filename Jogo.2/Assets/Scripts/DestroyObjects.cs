using System;
using UnityEditor.VersionControl;
using UnityEngine;

public class DestroyObjects : MonoBehaviour
{
    // Para desativar os objetos que vão cair
    public float time;
    void Update()
    {
        Destroy(gameObject, time);
    }
    // Para desativar os avisos:
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Alert"))
        {
            Destroy(collision.gameObject);
        }
    }
}
