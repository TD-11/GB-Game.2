using UnityEngine;

public class SimpleFall : MonoBehaviour
{
    // Impõe a velocidade nos objetos que vão cair
    public float speedFall;

    void Update()
    {
        //Aplica força e direção para o objeto que vai cair
        transform.position += Vector3.down * speedFall * Time.deltaTime;
    }
}
