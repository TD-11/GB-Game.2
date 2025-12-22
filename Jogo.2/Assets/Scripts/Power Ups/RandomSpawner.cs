using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Classe responsável por gerar (spawnar) objetos aleatoriamente no jogo
// Implementa o padrão Observer para reagir ao cronômetro
public class RandomSpawner : MonoBehaviour, ITimeObserver
{
    // =========================
    // SINGLETON
    // =========================

    // Instância única do RandomSpawner
    public static RandomSpawner Instance { get; private set; }

    // =========================
    // CONFIGURAÇÕES DE SPAWN
    // =========================

    // Intervalos de tempo entre os spawns (em segundos)
    public float intervalSpawnObstacle = 3f;
    public float intervalSpawnPowerLife = 13f;
    public float intervalSpawnShield = 11f;
    public float intervalSpawnShell = 5f;

    // Área horizontal de spawn
    public float widthArea = 8.4f;

    // Altura onde os objetos aparecem
    public float heightSpawnObject = 12f;

    // Altura onde os alertas aparecem
    public float heightSpawnAlert = 4f;

    // Próximo tempo em que cada objeto poderá ser spawnado
    private float nextSpawnTimeObstacle;
    private float nextSpawnTimeLife;
    private float nextSpawnTimeShield;
    private float nextSpawnTimeShell;

    // Controla se o spawner está ativo
    // É desativado quando o tempo do jogo acaba
    private bool canSpawn = true;

    // =========================
    // MÉTODOS UNITY
    // =========================

    void Awake()
    {
        // Implementação padrão do Singleton
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
        // Inicializa os tempos iniciais de spawn
        nextSpawnTimeObstacle = 0f;
        nextSpawnTimeLife = 9f;
        nextSpawnTimeShield = 11f;
        nextSpawnTimeShell = 5f;

        // Registra este spawner como observador do cronômetro
        StopWatch stopWatch = FindObjectOfType<StopWatch>();
        if (stopWatch != null)
        {
            stopWatch.AddObserver(this);
        }
    }

    void Update()
    {
        // Se o spawn estiver desativado, não faz nada
        if (!canSpawn) return;

        // Spawn de obstáculos
        if (Time.timeSinceLevelLoad >= nextSpawnTimeObstacle)
        {
            SpawnObstacleObject();
            nextSpawnTimeObstacle = Time.timeSinceLevelLoad + intervalSpawnObstacle;
        }

        // Spawn de vidas
        if (Time.timeSinceLevelLoad >= nextSpawnTimeLife)
        {
            SpawnLifeObject();
            nextSpawnTimeLife = Time.timeSinceLevelLoad + intervalSpawnPowerLife;
        }

        // Spawn de escudos
        if (Time.timeSinceLevelLoad >= nextSpawnTimeShield)
        {
            SpawnShieldObject();
            nextSpawnTimeShield = Time.timeSinceLevelLoad + intervalSpawnShield;
        }

        // Spawn de conchas (pontuação)
        if (Time.timeSinceLevelLoad >= nextSpawnTimeShell)
        {
            SpawnShellObject();
            nextSpawnTimeShell = Time.timeSinceLevelLoad + intervalSpawnShell;
        }
    }

    // =========================
    // MÉTODOS DE SPAWN
    // =========================

    // Gera obstáculos e seus alertas
    void SpawnObstacleObject()
    {
        // Sorteia posição horizontal
        float x = Random.Range(-widthArea / 2f, widthArea / 2f);

        // Define posições do objeto e do alerta
        Vector3 positionSpawnObject = new Vector3(x, heightSpawnObject, 0f);
        Vector3 positionSpawnAlert = new Vector3(x, heightSpawnAlert, 0f);

        // Spawna usando Object Pool
        ObjectPool.Instance.SpawnFromPool("Obstacle", positionSpawnObject, Quaternion.identity);
        ObjectPool.Instance.SpawnFromPool("ObstacleAlert", positionSpawnAlert, Quaternion.identity);
    }

    // Gera itens de vida e seus alertas
    void SpawnLifeObject()
    {
        float x = Random.Range(-widthArea / 2f, widthArea / 2f);
        Vector3 positionSpawnObject = new Vector3(x, heightSpawnObject, 0f);
        Vector3 positionSpawnAlert = new Vector3(x, heightSpawnAlert, 0f);

        ObjectPool.Instance.SpawnFromPool("Life", positionSpawnObject, Quaternion.identity);
        ObjectPool.Instance.SpawnFromPool("LifeAlert", positionSpawnAlert, Quaternion.identity);
    }

    // Gera escudos e seus alertas
    void SpawnShieldObject()
    {
        float x = Random.Range(-widthArea / 2f, widthArea / 2f);
        Vector3 positionSpawnObject = new Vector3(x, heightSpawnObject, 0f);

        // Pequeno ajuste vertical do alerta
        Vector3 positionSpawnAlert = new Vector3(x, heightSpawnAlert - 0.2f, 0f);

        ObjectPool.Instance.SpawnFromPool("Shield", positionSpawnObject, Quaternion.identity);
        ObjectPool.Instance.SpawnFromPool("ShieldAlert", positionSpawnAlert, Quaternion.identity);
    }

    // Gera conchas (pontuação) e seus alertas
    void SpawnShellObject()
    {
        float x = Random.Range(-widthArea / 2f, widthArea / 2f);
        Vector3 positionSpawnObject = new Vector3(x, heightSpawnObject, 0f);

        // Pequeno ajuste vertical do alerta
        Vector3 positionSpawnAlert = new Vector3(x, heightSpawnAlert - 0.3f, 0f);

        ObjectPool.Instance.SpawnFromPool("Shell", positionSpawnObject, Quaternion.identity);
        ObjectPool.Instance.SpawnFromPool("ShellAlert", positionSpawnAlert, Quaternion.identity);
    }

    // =========================
    // PADRÃO OBSERVER
    // =========================

    // Chamado sempre que o tempo do cronômetro muda
    public void OnTimeChanged(float timeLeft)
    {
        // Aumenta a dificuldade conforme o tempo diminui
        if (timeLeft < 60f)
            intervalSpawnObstacle = 2.5f;

        if (timeLeft < 30f)
            intervalSpawnObstacle = 2.0f;
    }

    // Chamado quando o tempo acaba
    public void OnTimeEnded()
    {
        // Interrompe completamente o spawn de objetos
        canSpawn = false;
    }
}