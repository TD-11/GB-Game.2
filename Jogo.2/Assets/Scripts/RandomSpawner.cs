using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public GameObject player;
    public GameObject dangerousObject;
    public GameObject powerUpObject;
    public GameObject badAlert;// Objeto que vai cair
    public GameObject goodAlert;// Objeto que vai cair
    public float intervaloSpawn1;// Tempo entre os spawns
    public float intervaloSpawn2;// Tempo entre os spawns
    public float areaLargura;// Largura da área de spawn
    public float alturaSpawn1;// Altura em Y (onde os objetos aparecem)  
    public float alturaSpawn2;// Altura em Y (onde os objetos aparecem)

    private float tempoProximoSpawn1 = 0f;
    private float tempoProximoSpawn2 = 10f;

    void Update()
    {
            if (Time.time >= tempoProximoSpawn1)
            {
                SpawnDangerousObject();
                tempoProximoSpawn1 = Time.time + intervaloSpawn1;
            }
            if (Time.time >= tempoProximoSpawn2)
            {
                SpawnPowerUpObject();
                tempoProximoSpawn2 = Time.time + intervaloSpawn2;
            }
    }

    void SpawnDangerousObject()
    {
        float x = Random.Range(-areaLargura / 2f, areaLargura / 2f); // centro = 0
        
        Vector3 posicaoSpawn1 = new Vector3(x, alturaSpawn1, 0f);
        Vector3 posicaoSpawn2 = new Vector3(x, alturaSpawn2, 0f);
        
        Instantiate(dangerousObject, posicaoSpawn1, Quaternion.identity);
        Instantiate(badAlert, posicaoSpawn2, Quaternion.identity);
    }

    void SpawnPowerUpObject()
    {
        float x = Random.Range(-areaLargura / 2f, areaLargura / 2f); // centro = 0
        Vector3 posicaoSpawn1 = new Vector3(x, alturaSpawn1, 0f);
        Vector3 posicaoSpawn2 = new Vector3(x, alturaSpawn2, 0f);
        
        Instantiate(powerUpObject, posicaoSpawn1, Quaternion.identity);
        Instantiate(goodAlert, posicaoSpawn2, Quaternion.identity);
    }
}
