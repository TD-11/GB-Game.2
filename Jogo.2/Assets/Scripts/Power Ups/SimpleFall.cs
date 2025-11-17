using UnityEngine;

public class SimpleFall : MonoBehaviour
{
    // Velocidade com que o objeto vai cair.
    // Pode ser ajustada no Inspector.
    public float speedFall;

    void Update()
    {
        // Move o objeto continuamente para baixo.
        // Vector3.down = (0, -1, 0)
        // speedFall define a velocidade
        // Time.deltaTime garante movimento suave e independente do FPS.
        transform.position += Vector3.down * speedFall * Time.deltaTime;
    }
}
