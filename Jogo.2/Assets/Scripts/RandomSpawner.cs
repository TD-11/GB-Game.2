using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RandomSpawner : MonoBehaviour
{
    // === Singleton ===
    public static RandomSpawner Instance { get; private set; }

    void Awake()
    {
        // Garante que exista apenas um RandomSpawner ativo
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // === Configurações de spawn ===
    public float intervalSpawnObstacle = 3f;
    public float intervalSpawnPowerLife = 13f;
    public float intervalSpawnShield = 11f;
    public float intervalSpawnShell = 5f;
    public float widthArea = 8.4f;
    public float heightSpawnObject = 12f;
    public float heightSpawnAlert = 4f;

    private float nextSpawnTimeObstacle;
    private float nextSpawnTimeLife;
    private float nextSpawnTimeShield;
    private float nextSpawnTimeShell;

    void Start()
    {
        nextSpawnTimeObstacle = 0f;
        nextSpawnTimeLife = 9f;
        nextSpawnTimeShield = 11f;
        nextSpawnTimeShell = 5f;
    }

    void Update()
    {
        if (Time.timeSinceLevelLoad >= nextSpawnTimeObstacle)
        {
            SpawnObstacleObject();
            nextSpawnTimeObstacle = Time.timeSinceLevelLoad + intervalSpawnObstacle;
        }

        if (Time.timeSinceLevelLoad >= nextSpawnTimeLife)
        {
            SpawnLifeObject();
            nextSpawnTimeLife = Time.timeSinceLevelLoad + intervalSpawnPowerLife;
        }

        if (Time.timeSinceLevelLoad >= nextSpawnTimeShield)
        {
            SpawnShieldObject();
            nextSpawnTimeShield = Time.timeSinceLevelLoad + intervalSpawnShield;
        }

        if (Time.timeSinceLevelLoad >= nextSpawnTimeShell)
        {
            SpawnShellObject();
            nextSpawnTimeShell = Time.timeSinceLevelLoad + intervalSpawnShell;
        }
    }

    void SpawnObstacleObject()
    {
        float x = Random.Range(-widthArea / 2f, widthArea / 2f);
        Vector3 positionSpawnObject = new Vector3(x, heightSpawnObject, 0f);
        Vector3 positionSpawnAlert = new Vector3(x, heightSpawnAlert, 0f);

        ObjectPool.Instance.SpawnFromPool("Obstacle", positionSpawnObject, Quaternion.identity);
        ObjectPool.Instance.SpawnFromPool("ObstacleAlert", positionSpawnAlert, Quaternion.identity);
    }

    void SpawnLifeObject()
    {
        float x = Random.Range(-widthArea / 2f, widthArea / 2f);
        Vector3 positionSpawnObject = new Vector3(x, heightSpawnObject, 0f);
        Vector3 positionSpawnAlert = new Vector3(x, heightSpawnAlert, 0f);

        ObjectPool.Instance.SpawnFromPool("Life", positionSpawnObject, Quaternion.identity);
        ObjectPool.Instance.SpawnFromPool("LifeAlert", positionSpawnAlert, Quaternion.identity);
    }

    void SpawnShieldObject()
    {
        float x = Random.Range(-widthArea / 2f, widthArea / 2f);
        Vector3 positionSpawnObject = new Vector3(x, heightSpawnObject, 0f);
        Vector3 positionSpawnAlert = new Vector3(x, heightSpawnAlert - 0.2f, 0f);

        ObjectPool.Instance.SpawnFromPool("Shield", positionSpawnObject, Quaternion.identity);
        ObjectPool.Instance.SpawnFromPool("ShieldAlert", positionSpawnAlert, Quaternion.identity);
    }

    void SpawnShellObject()
    {
        float x = Random.Range(-widthArea / 2f, widthArea / 2f);
        Vector3 positionSpawnObject = new Vector3(x, heightSpawnObject, 0f);
        Vector3 positionSpawnAlert = new Vector3(x, heightSpawnAlert - 0.3f, 0f);

        ObjectPool.Instance.SpawnFromPool("Shell", positionSpawnObject, Quaternion.identity);
        ObjectPool.Instance.SpawnFromPool("ShellAlert", positionSpawnAlert, Quaternion.identity);
    }
}