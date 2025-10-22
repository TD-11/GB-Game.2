using UnityEngine;

public class TREMOR : MonoBehaviour
{

    private Animator animator;
    
    
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void playTremor()
    {
        animator.SetTrigger("Tremor");
    }

    void Update()
    {
        
    }
}
