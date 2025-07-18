using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public GameObject dangerousObject;// Objeto que irá dar dano
    public GameObject powerUpObject;// Objeto que ajudará o jogador
    public GameObject badAlert;// Alerta de objeto ruim
    public GameObject goodAlert;// Alerta de power 
    
    
    public float intervalSpawn1 = 5f;// Tempo entre os spawns
    public float intervalSpawn2 = 15f;// Tempo entre os spawns
    public float widthArea = 8.4f;// Largura da área de spawn
    public float heightSpawn1 = 12f;// Altura em Y (onde os objetos aparecem)  
    public float heightSpawn2 = 4f;// Altura em Y (onde os objetos aparecem)

    public bool Fall = true;// Vai servir para liberar a queda dos objetos   

    // Define quando os objetos vão cair
    private float nextSpawnTime1 = 0f;
    private float nextSpawnTime2 = 10f;

    void Update()
    {
        // Se a variavel "Fall" for verdadeira:
        if (Fall)
        {
            // Põe as funções em prática de acordo com tempo pré-definido
            if (Time.time >= nextSpawnTime1)
            {
                SpawnDangerousObject();
                nextSpawnTime1 = Time.time + intervalSpawn1;
            }

            if (Time.time >= nextSpawnTime2)
            {
                SpawnPowerUpObject();
                nextSpawnTime2 = Time.time + intervalSpawn2;
            }
        }
    }

    void SpawnDangerousObject()
    {
        float x = Random.Range(-widthArea / 2f, widthArea / 2f);// Define um valor aleatoriamente na zona de spawn 
        
        Vector3 positionSpawn1 = new Vector3(x, heightSpawn1, 0f);// Define o ponto de spawn do objeto
        Vector3 positionSpawn2 = new Vector3(x, heightSpawn2, 0f);// Define o ponto de spawn do objeto

        Instantiate(dangerousObject, positionSpawn1, Quaternion.identity);// Instancia o objeto
        Instantiate(badAlert, positionSpawn2, Quaternion.identity);// Instancia o objeto
    }

    void SpawnPowerUpObject()
    {
        float x = Random.Range(-widthArea / 2f, widthArea / 2f);// Define um valor aleatoriamente na zona de spawn
        
        Vector3 positionSpawn1 = new Vector3(x, heightSpawn1, 0f);// Define o ponto de spawn do objeto
        Vector3 positionSpawn2 = new Vector3(x, heightSpawn2, 0f);// Define o ponto de spawn do objeto
        
        Instantiate(powerUpObject, positionSpawn1, Quaternion.identity);// Instancia o objeto
        Instantiate(goodAlert, positionSpawn2, Quaternion.identity);// Instancia o objeto
    }
}
