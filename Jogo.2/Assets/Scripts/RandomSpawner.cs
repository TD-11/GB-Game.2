using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RandomSpawner : MonoBehaviour, ITimeObserver
{
    // === Singleton ===
    public static RandomSpawner Instance { get; private set; }

    // === Controle de spawn ===
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

    private bool canSpawn = true; // usado para parar o spawn quando o tempo acabar

    void Awake()
    {
        // Singleton padrão
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        nextSpawnTimeObstacle = 0f;
        nextSpawnTimeLife = 9f;
        nextSpawnTimeShield = 11f;
        nextSpawnTimeShell = 5f;

        // Registra o spawner como observador do cronômetro
        StopWatch stopWatch = FindObjectOfType<StopWatch>();
        if (stopWatch != null)
        {
            stopWatch.AddObserver(this);
            Debug.Log("RandomSpawner registrado como observador do cronômetro.");
        }
    }

    void Update()
    {
        if (!canSpawn) return; // Para tudo quando o tempo acaba

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

    // Padrão Observer

    // Quando o tempo muda (ex: pode aumentar dificuldade)
    public void OnTimeChanged(float timeLeft)
    {
        // Exemplo: deixar o jogo mais difícil conforme o tempo passa
        if (timeLeft < 60f)
            intervalSpawnObstacle = 2.5f; // mais obstáculos
        if (timeLeft < 30f)
            intervalSpawnObstacle = 2.0f; // ainda mais rápidos
    }

    // Quando o tempo acaba
    public void OnTimeEnded()
    {
        canSpawn = false;
    }
}