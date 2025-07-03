using UnityEngine;

public class FallingObjects1 : MonoBehaviour
{
    public GameObject fallingObjectPrefab; // Prefab do objeto que vai cair
    public GameObject alert;
    public Transform player; // Referência ao jogador
    public float spawnInterval = 7f; // Tempo entre cada instância
    public float spawnRangeX = 5f; // Largura horizontal do spawn
    public float spawnHeight = 10f; // Altura onde os objetos aparecerão
    public float spawnAlert = 1f; // Altura onde os objetos aparecerão

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
        float x = Random.Range(-spawnRangeX, spawnRangeX);
        Vector3 spawnPosition1 = new Vector3(player.position.x + x, player.position.y + spawnHeight, 0f);
        Vector3 spawnPosition2 = new Vector3(player.position.x + x, player.position.y + spawnAlert, 0f);
        Instantiate(fallingObjectPrefab, spawnPosition1, Quaternion.identity);
        Instantiate(alert, spawnPosition2, Quaternion.identity);
    }
}
