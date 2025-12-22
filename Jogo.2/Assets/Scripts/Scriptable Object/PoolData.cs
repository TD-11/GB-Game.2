using UnityEngine;

// Permite criar este ScriptableObject pelo menu do Unity
// Assets > Create > Pooling > PoolData
[CreateAssetMenu(fileName = "NewPoolData", menuName = "Pooling/PoolData")]
public class PoolData : ScriptableObject
{
    // Identificador da pool
    // Usado pelo ObjectPool para localizar a pool correta
    public string tag;

    // Prefab que será instanciado e reutilizado na pool
    public GameObject prefab;

    // Quantidade inicial de objetos que serão criados na pool
    public int size;
}