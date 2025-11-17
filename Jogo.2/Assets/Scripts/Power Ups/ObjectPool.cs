using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    // Instância estática para permitir acesso global ao Pool
    public static ObjectPool Instance;

    [Header("Lista de Pools configuradas")]
    public List<PoolData> pools; 
    // Essa lista contém as configurações das pools via ScriptableObject:
    // - tag do objeto
    // - prefab que será instanciado
    // - quantidade inicial

    // Dicionário que armazena cada pool por sua tag, facilitando acesso e busca
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    void Awake()
    {
        // Define a instância global para fácil acesso a partir de outros scripts
        Instance = this;
    }

    void Start()
    {
        // Inicializa o dicionário que guardará todas as filas de objetos
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        // Para cada pool configurada no ScriptableObject
        foreach (PoolData pool in pools)
        {
            // Cria uma fila (Queue) que armazenará os objetos reutilizáveis
            Queue<GameObject> objectPool = new Queue<GameObject>();

            // Instancia o número inicial de objetos definido em pool.size
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab); // Cria o objeto
                obj.SetActive(false);                       // Desativa para não aparecer inicialmente
                objectPool.Enqueue(obj);                    // Adiciona à fila
            }

            // Adiciona essa fila ao dicionário com sua respectiva tag
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    // Método para pegar um objeto da Pool
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        // Verifica se existe uma pool com essa tag
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool com tag " + tag + " não existe.");
            return null;
        }

        // Se a pool tem objetos disponíveis, remove o primeiro da fila
        // Caso contrário, instancia um novo objeto (pool dinâmica)
        GameObject objectToSpawn = poolDictionary[tag].Count > 0
            ? poolDictionary[tag].Dequeue()
            : Instantiate(GetPoolPrefab(tag));

        // Ativa o objeto e atualiza sua posição e rotação
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        return objectToSpawn;
    }

    // Retorna um objeto para a pool
    public void ReturnToPool(string tag, GameObject obj)
    {
        obj.SetActive(false);             // Desativa o objeto ao devolver
        poolDictionary[tag].Enqueue(obj); // Coloca novamente na fila da pool correspondente
    }

    // Busca o prefab baseado na tag da pool
    private GameObject GetPoolPrefab(string tag)
    {
        foreach (PoolData pool in pools)
        {
            if (pool.tag == tag)
            {
                return pool.prefab; // Retorna o prefab certo
            }
        }
        return null; // Caso algo esteja configurado errado
    }
}