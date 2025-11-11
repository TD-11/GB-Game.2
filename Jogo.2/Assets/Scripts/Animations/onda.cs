
using UnityEngine;

public class WaveController : MonoBehaviour
{
    void Start()
    {
        // Acha todos os objetos que têm um componente Animator na cena
        Animator[] ondas = FindObjectsOfType<Animator>();

        // Ativa a animação em todas as ondas
        foreach (Animator anim in ondas)
        {
            anim.SetTrigger("Mexer"); // "Mexer" é o nome do Trigger no Animator
        }
    }
}