using UnityEngine;

// Classe responsável por acionar uma animação de tremor através de um trigger no Animator
public class Tremor : MonoBehaviour
{
    // Referência para o componente Animator anexado ao GameObject
    private Animator animator;
    
    void Start()
    {
        // Obtém o componente Animator no mesmo GameObject
        animator = GetComponent<Animator>();
    }

    // Método público que dispara a animação de tremor
    public void playTremor()
    {
        // Ativa o trigger chamado "Tremor" configurado no Animator
        animator.SetTrigger("Tremor");
    }
}
