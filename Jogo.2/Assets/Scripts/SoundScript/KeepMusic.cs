using UnityEngine;

public class KeepMusic : MonoBehaviour
{
    public static KeepMusic instance; 
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
