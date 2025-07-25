using UnityEngine;
using TMPro;

public class RandomSpawner : MonoBehaviour
{
    public GameObject obstacleObject;// Objeto que irá dar dano
    public GameObject obstacleAlert;// Alerta de objeto ruim
    //public GameObject lifeObject;// Objeto que ajudará o jogador
    public GameObject lifeAlert;// Alerta de power
    //public GameObject shieldObject;// Power up que dará um escudo ao player
    public GameObject shieldAlert;// Aviso do power up
    //public GameObject shellObject;
    public GameObject shellAlert;
    
    public float intervalSpawnObstacle = 5f;// Tempo entre os spawns
    public float intervalSpawnPowerLife = 15f;// Tempo entre os spawns
    public float intervalSpawnShield = 15f;// Tempo entre os spawns
    public float intervalSpawnShell = 5f;// Tempo entre os spawns
    public float widthArea = 8.4f;// Largura da área de spawn
    public float heightSpawnObject = 12f;// Altura em Y (onde os objetos aparecem)  
    public float heightSpawnAlert = 4f;// Altura em Y (onde os objetos aparecem)

    public bool Fall = true;// Vai servir para liberar a queda dos objetos   

    // Define quando os objetos vão cair
    private float nextSpawnTimeObstacle = 0f;
    private float nextSpawnTimeLife = 10f;
    private float nextSpawnTimeShield = 10f;
    private float nextSpawnTimeShell = 5f;

    void Update()
    {
        // Se a variavel "Fall" for verdadeira:
        if (Fall)
        {
            // Põe as funções em prática de acordo com tempo pré-definido
            if (Time.time >= nextSpawnTimeObstacle)
            {
                SpawnDangerousObject();
                nextSpawnTimeObstacle = Time.time + intervalSpawnObstacle;
            }

            if (Time.time >= nextSpawnTimeLife)
            {
                SpawnPowerUpObject();
                nextSpawnTimeLife = Time.time + intervalSpawnPowerLife;
            }
            
            if (Time.time >= nextSpawnTimeShield)
            {
                SpawnShieldObject();
                nextSpawnTimeShield = Time.time + intervalSpawnShield;
            }
            
            if (Time.time >= nextSpawnTimeShell)
            {
                SpawnShellObject();
                nextSpawnTimeShell = Time.time + intervalSpawnShell;
            }
        }
    }

    void SpawnDangerousObject()
    {
        float x = Random.Range(-widthArea / 2f, widthArea / 2f);// Define um valor aleatoriamente na zona de spawn 
        
        Vector3 positionSpawnObject = new Vector3(x, heightSpawnObject, 0f);// Define o ponto de spawn do objeto
        Vector3 positionSpawnAlert = new Vector3(x, heightSpawnAlert, 0f);// Define o ponto de spawn do objeto

        ObjectPool.Instance.SpawnFromPool("Danger", positionSpawnObject, Quaternion.identity);// Instancia o objeto
        Instantiate(obstacleAlert, positionSpawnAlert, Quaternion.identity);// Instancia o objeto
    }

    void SpawnPowerUpObject()
    {
        float x = Random.Range(-widthArea / 2f, widthArea / 2f);// Define um valor aleatoriamente na zona de spawn
        
        Vector3 positionSpawnObject = new Vector3(x, heightSpawnObject, 0f);// Define o ponto de spawn do objeto
        Vector3 positionSpawnAlert = new Vector3(x, heightSpawnAlert, 0f);// Define o ponto de spawn do objeto
        
        ObjectPool.Instance.SpawnFromPool("Life", positionSpawnObject, Quaternion.identity);// Instancia o objeto
        ObjectPool.Instance.SpawnFromPool("LifeAlert", positionSpawnAlert, Quaternion.identity);// Instancia o objeto
    }
    
    void SpawnShieldObject()
    {
        float x = Random.Range(-widthArea / 2f, widthArea / 2f);// Define um valor aleatoriamente na zona de spawn
        
        Vector3 positionSpawnObject = new Vector3(x, heightSpawnObject, 0f);// Define o ponto de spawn do objeto
        Vector3 positionSpawnAlert = new Vector3(x, heightSpawnAlert, 0f);// Define o ponto de spawn do objeto
        
        ObjectPool.Instance.SpawnFromPool("Protection", positionSpawnObject, Quaternion.identity);// Instancia o objeto
        Instantiate(shieldAlert, positionSpawnAlert, Quaternion.identity);// Instancia o objeto
    }
    
    void SpawnShellObject()
    {
        float x = Random.Range(-widthArea / 2f, widthArea / 2f);// Define um valor aleatoriamente na zona de spawn
        
        Vector3 positionSpawnObject = new Vector3(x, heightSpawnObject, 0f);// Define o ponto de spawn do objeto
        Vector3 positionSpawnAlert = new Vector3(x, heightSpawnAlert, 0f);// Define o ponto de spawn do objeto
        
        ObjectPool.Instance.SpawnFromPool("Shell", positionSpawnObject, Quaternion.identity);// Instancia o objeto
        Instantiate(shellAlert, positionSpawnAlert, Quaternion.identity);// Instancia o objeto
    }
}
