using UnityEngine;

public class SimpleFall : MonoBehaviour
{
    public float speed = 3f;

    void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;
    }
}
