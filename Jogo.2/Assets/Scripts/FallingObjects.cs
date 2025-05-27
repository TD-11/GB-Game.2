using UnityEngine;

public class FallingObjects : MonoBehaviour
{
    public GameObject fallingObjectPrefab; // Prefab do objeto que vai cair
    public Transform player; // Referência ao jogador
    public float spawnInterval = 2f; // Tempo entre cada instância
    public float spawnRangeX = 5f; // Largura horizontal do spawn
    public float spawnHeight = 10f; // Altura onde os objetos aparecerão

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnObject();
        }
    }

    void SpawnObject()
    {
        Vector3 spawnPosition = new Vector3(player.position.x + Random.Range(-spawnRangeX, spawnRangeX), player.position.y + spawnHeight, 0f);

        Instantiate(fallingObjectPrefab, spawnPosition, Quaternion.identity);
    }
}
