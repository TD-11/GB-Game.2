using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RandomSpawner : MonoBehaviour
{
    public float intervalSpawnObstacle = 3f;// Tempo entre os spawns
    public float intervalSpawnPowerLife = 13f;// Tempo entre os spawns
    public float intervalSpawnShield = 11f;// Tempo entre os spawns
    public float intervalSpawnShell = 5f;// Tempo entre os spawns
    public float widthArea = 8.4f;// Largura da área de spawn
    public float heightSpawnObject = 12f;// Altura em Y (onde os objetos aparecem)  
    public float heightSpawnAlert = 4f;// Altura em Y (onde os objetos aparece

    // Define quando os objetos vão cair
    private float nextSpawnTimeObstacle = 0f;
    private float nextSpawnTimeLife = 9f;
    private float nextSpawnTimeShield = 11f;
    private float nextSpawnTimeShell = 5f;
    

    void Update()
    {
        // Põe as funções em prática de acordo com tempo pré-definido
        if (Time.time >= nextSpawnTimeObstacle)
        {
            SpawnObstacleObject();
            nextSpawnTimeObstacle = Time.time + intervalSpawnObstacle;
        }

        if (Time.time >= nextSpawnTimeLife)
        {
            SpawnLifeObject();
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
    

    void SpawnObstacleObject()
    {
        float x = Random.Range(-widthArea / 2f, widthArea / 2f);// Define um valor aleatoriamente na zona de spawn 
        
        Vector3 positionSpawnObject = new Vector3(x, heightSpawnObject, 0f);// Define o ponto de spawn do objeto
        Vector3 positionSpawnAlert = new Vector3(x, heightSpawnAlert, 0f);// Define o ponto de spawn do objeto

        var obj = ObjectPool.Instance.SpawnFromPool("Obstacle", positionSpawnObject, Quaternion.identity);// Instancia o objeto
        var alert = ObjectPool.Instance.SpawnFromPool("ObstacleAlert", positionSpawnAlert, Quaternion.identity);
    }

    void SpawnLifeObject()
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

        Vector3 positionSpawnObject = new Vector3(x, heightSpawnObject, 0f); // Define o ponto de spawn do objeto
        Vector3 positionSpawnAlert = new Vector3(x, heightSpawnAlert, 0f); // Define o ponto de spawn do objeto

        ObjectPool.Instance.SpawnFromPool("Shield", positionSpawnObject, Quaternion.identity); // Instancia o objeto
        ObjectPool.Instance.SpawnFromPool("ShieldAlert", positionSpawnAlert, Quaternion.identity); // Instancia o objeto
    }

    void SpawnShellObject()
    {
        float x = Random.Range(-widthArea / 2f, widthArea / 2f);// Define um valor aleatoriamente na zona de spawn 
        
        Vector3 positionSpawnObject = new Vector3(x, heightSpawnObject, 0f);// Define o ponto de spawn do objeto
        Vector3 positionSpawnAlert = new Vector3(x, heightSpawnAlert, 0f);// Define o ponto de spawn do objeto
        
        ObjectPool.Instance.SpawnFromPool("Shell", positionSpawnObject, Quaternion.identity);// Instancia o objeto
        ObjectPool.Instance.SpawnFromPool("ShellAlert", positionSpawnAlert, Quaternion.identity);// Instancia o objeto
    }
}
