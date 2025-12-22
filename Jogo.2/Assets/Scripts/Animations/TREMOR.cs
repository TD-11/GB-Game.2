using UnityEngine;

// Classe responsável por controlar uma animação de tremor
public class Tremor : MonoBehaviour
{
    // Referência ao componente Animator do objeto
    private Animator animator;
    
    void Start()
    {
        // Obtém o componente Animator anexado ao mesmo GameObject
        animator = GetComponent<Animator>();
    }

    // Método público que dispara a animação de tremor
    // Pode ser chamado por outros scripts ou eventos da UI
    public void playTremor()
    {
        // Ativa o Trigger "Tremor" configurado no Animator
        animator.SetTrigger("Tremor");
    }
}
