using UnityEngine;

// Cria uma opção no menu do Unity para gerar novos arquivos do tipo "PoolData"
// Menu: Create -> Pooling -> PoolData
[CreateAssetMenu(fileName = "NewPoolData", menuName = "Pooling/PoolData")]
public class PoolData : ScriptableObject
{
    // Identificador único da pool (nome usado no ObjectPool para encontrá-la)
    public string tag;

    // Prefab que será instanciado e armazenado na pool
    public GameObject prefab;

    // Quantidade inicial de objetos que essa pool vai criar ao iniciar
    public int size;
}