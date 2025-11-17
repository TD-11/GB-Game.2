using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RandomSpawner : MonoBehaviour, ITimeObserver
{
    // === Singleton ===
    // Permite que outros scripts acessem o spawner sem precisar arrastar no Inspector
    public static RandomSpawner Instance { get; private set; }

    // === Controle de spawn ===
    public float intervalSpawnObstacle = 3f;   // Intervalo para spawn de obstáculos
    public float intervalSpawnPowerLife = 13f; // Intervalo para spawn de vidas
    public float intervalSpawnShield = 11f;    // Intervalo para spawn de escudo
    public float intervalSpawnShell = 5f;      // Intervalo para spawn da casca
    public float widthArea = 8.4f;             // Área horizontal de spawn
    public float heightSpawnObject = 12f;      // Altura onde o objeto aparece
    public float heightSpawnAlert = 4f;        // Altura onde o alerta aparece

    // Próximos tempos em que cada item deve aparecer
    private float nextSpawnTimeObstacle;
    private float nextSpawnTimeLife;
    private float nextSpawnTimeShield;
    private float nextSpawnTimeShell;

    // Controle para impedir spawn quando o tempo do jogo acaba
    private bool canSpawn = true;

    void Awake()
    {
        // Implementação padrão do Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Evita duplicatas
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        // Define os primeiros tempos de spawn
        nextSpawnTimeObstacle = 0f;
        nextSpawnTimeLife = 9f;
        nextSpawnTimeShield = 11f;
        nextSpawnTimeShell = 5f;

        // Registra este spawner como observador do StopWatch (Observer Pattern)
        StopWatch stopWatch = FindObjectOfType<StopWatch>();
        if (stopWatch != null)
        {
            stopWatch.AddObserver(this);
        }
    }

    void Update()
    {
        // Se o tempo acabou, para completamente os spawns
        if (!canSpawn) return;

        // Checa cada tipo de objeto e verifica se já passou o tempo necessário
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

    // === Funções de Spawn ===

    void SpawnObstacleObject()
    {
        // Sorteia posição X dentro da área permitida
        float x = Random.Range(-widthArea / 2f, widthArea / 2f);

        // Define posição para o objeto e para o alerta visual
        Vector3 posObj = new Vector3(x, heightSpawnObject, 0f);
        Vector3 posAlert = new Vector3(x, heightSpawnAlert, 0f);

        // Usa o Object Pool para spawnar sem criar objetos novos
        ObjectPool.Instance.SpawnFromPool("Obstacle", posObj, Quaternion.identity);
        ObjectPool.Instance.SpawnFromPool("ObstacleAlert", posAlert, Quaternion.identity);
    }

    void SpawnLifeObject()
    {
        float x = Random.Range(-widthArea / 2f, widthArea / 2f);
        Vector3 posObj = new Vector3(x, heightSpawnObject, 0f);
        Vector3 posAlert = new Vector3(x, heightSpawnAlert, 0f);

        ObjectPool.Instance.SpawnFromPool("Life", posObj, Quaternion.identity);
        ObjectPool.Instance.SpawnFromPool("LifeAlert", posAlert, Quaternion.identity);
    }

    void SpawnShieldObject()
    {
        float x = Random.Range(-widthArea / 2f, widthArea / 2f);
        Vector3 posObj = new Vector3(x, heightSpawnObject, 0f);
        Vector3 posAlert = new Vector3(x, heightSpawnAlert - 0.2f, 0f); // Ajuste fino na altura do alerta

        ObjectPool.Instance.SpawnFromPool("Shield", posObj, Quaternion.identity);
        ObjectPool.Instance.SpawnFromPool("ShieldAlert", posAlert, Quaternion.identity);
    }

    void SpawnShellObject()
    {
        float x = Random.Range(-widthArea / 2f, widthArea / 2f);
        Vector3 posObj = new Vector3(x, heightSpawnObject, 0f);
        Vector3 posAlert = new Vector3(x, heightSpawnAlert - 0.3f, 0f);

        ObjectPool.Instance.SpawnFromPool("Shell", posObj, Quaternion.identity);
        ObjectPool.Instance.SpawnFromPool("ShellAlert", posAlert, Quaternion.identity);
    }

    // === Implementação do Observer Pattern ===

    // É chamado sempre que o tempo do StopWatch muda
    public void OnTimeChanged(float timeLeft)
    {
        // Exemplo de aumento de dificuldade automatizado
        if (timeLeft < 60f)
            intervalSpawnObstacle = 2.5f; // mais obstáculos

        if (timeLeft < 30f)
            intervalSpawnObstacle = 2.0f; // dificuldade aumenta ainda mais
    }

    // É chamado quando o StopWatch avisa que o tempo chegou a zero
    public void OnTimeEnded()
    {
        canSpawn = false; // Para totalmente os spawns
    }
}