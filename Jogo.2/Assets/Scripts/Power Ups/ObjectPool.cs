using System.Collections.Generic;
using UnityEngine;

// Classe responsável por gerenciar o sistema de Object Pooling
// Utiliza o padrão Singleton para acesso global
public class ObjectPool : MonoBehaviour
{
    // Instância única do ObjectPool (Singleton)
    public static ObjectPool Instance;

    [Header("Lista de Pools configuradas")]

    // Lista de configurações de pools (usa ScriptableObject: PoolData)
    public List<PoolData> pools;

    // Dicionário que associa uma tag a uma fila de objetos
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    void Awake()
    {
        // Inicializa a instância única
        Instance = this;
    }

    void Start()
    {
        // Inicializa o dicionário de pools
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        // Percorre todas as pools configuradas no Inspector
        foreach (PoolData pool in pools)
        {
            // Cria uma fila para armazenar os objetos dessa pool
            Queue<GameObject> objectPool = new Queue<GameObject>();

            // Instancia a quantidade inicial de objetos definida
            for (int i = 0; i < pool.size; i++)
            {
                // Cria o objeto a partir do prefab
                GameObject obj = Instantiate(pool.prefab);

                // Mantém o objeto desativado até ser utilizado
                obj.SetActive(false);

                // Adiciona o objeto à fila da pool
                objectPool.Enqueue(obj);
            }

            // Associa a fila criada à tag da pool
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    // =========================
    // SPAWN DE OBJETOS
    // =========================

    // Retorna um objeto da pool correspondente à tag informada
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        // Verifica se a pool existe
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool com tag " + tag + " não existe.");
            return null;
        }

        // Se houver objetos disponíveis na fila, reutiliza
        // Caso contrário, instancia um novo objeto dinamicamente
        GameObject objectToSpawn = poolDictionary[tag].Count > 0
            ? poolDictionary[tag].Dequeue()
            : Instantiate(GetPoolPrefab(tag));

        // Ativa o objeto e define posição e rotação
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        return objectToSpawn;
    }

    // =========================
    // RETORNO À POOL
    // =========================

    // Devolve um objeto para sua respectiva pool
    public void ReturnToPool(string tag, GameObject obj)
    {
        // Desativa o objeto antes de armazenar
        obj.SetActive(false);

        // Reinsere o objeto na fila correspondente
        poolDictionary[tag].Enqueue(obj);
    }

    // =========================
    // MÉTODO AUXILIAR
    // =========================

    // Obtém o prefab associado a uma determinada tag
    private GameObject GetPoolPrefab(string tag)
    {
        // Procura na lista de pools
        foreach (PoolData pool in pools)
        {
            if (pool.tag == tag)
            {
                return pool.prefab;
            }
        }

        // Retorna null caso não encontre
        return null;
    }
}