using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [System.Serializable]// Faz com que seja possivel fazer modificações direto no inspetor
    
    // Clase que guarda as informçoes do objeto e a quantidade que deseja usar 
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public static ObjectPool Instance;

    public List<Pool> pools;// Lista para armazenar os objetos
    public Dictionary<string, Queue<GameObject>> poolDictionary;// Dicionario de identificação que guarda uma fila de objetos (Queue)

    void Awake()
    {
        Instance = this;// Instancia a classe "ObjectPool"
    }

    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();// Cria a fila

        foreach (Pool pool in pools)// Verifica cada elemento da fila
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    // Instancia cada objeto da fila de acordo com a tag, posição e rotação
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool com tag " + tag + " não existe.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Count > 0
            ? poolDictionary[tag].Dequeue()
            : Instantiate(GetPoolPrefab(tag));

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        return objectToSpawn;
    }

    // Sistema para devolver o objeto para a "pool"
    public void ReturnToPool(string tag, GameObject obj)
    {
        obj.SetActive(false);
        poolDictionary[tag].Enqueue(obj);
    }

    // Pega o "prefab" do objeto
    private GameObject GetPoolPrefab(string tag)
    {
        foreach (Pool pool in pools)
        {
            if (pool.tag == tag)
            {
                return pool.prefab;
            }
        }
        return null;
    }
}
