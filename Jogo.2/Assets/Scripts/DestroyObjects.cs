using System;
using UnityEditor.VersionControl;
using UnityEngine;

public class DestroyObjects : MonoBehaviour
{
    public float time;
    void Update()
    {
        Destroy(gameObject, time);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Alert"))
        {
            Destroy(collision.gameObject);
        }
    }
}
