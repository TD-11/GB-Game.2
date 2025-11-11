using UnityEngine;

[CreateAssetMenu(fileName = "NewPoolData", menuName = "Pooling/PoolData")]
public class PoolData : ScriptableObject
{
    public string tag;
    public GameObject prefab;
    public int size;
}