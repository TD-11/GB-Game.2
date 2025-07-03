using System;
using UnityEditor.VersionControl;
using UnityEngine;

public class DestroyObjects : MonoBehaviour
{
    void Update()
    {
        Destroy(gameObject, 2f);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Alert"))
        {
            Destroy(collision.gameObject);
        }
    }
}
