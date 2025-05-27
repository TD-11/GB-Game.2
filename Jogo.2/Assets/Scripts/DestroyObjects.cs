using UnityEditor.VersionControl;
using UnityEngine;

public class DestroyObjects : MonoBehaviour
{
    void Update()
    {
        Destroy(gameObject, 2f);
    }
}
