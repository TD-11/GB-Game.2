using System;
using System.Collections.Generic;
using UnityEngine;

public class UnityMainThreadDispatcher : MonoBehaviour
{
    // =========================
    //        SINGLETON
    // =========================
    // Instância global do dispatcher
    public static UnityMainThreadDispatcher Instance;

    // Fila de ações que devem ser executadas na thread principal
    // static para poder ser acessada de qualquer lugar
    private static readonly Queue<Action> _actions = new Queue<Action>();

    void Awake()
    {
        // Garante apenas uma instância do dispatcher
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Mantém o dispatcher ativo entre trocas de cena
        DontDestroyOnLoad(gameObject);
    }

    // =========================
    //     ENFILEIRAR AÇÕES
    // =========================
    // Permite que qualquer thread (ex: serial, rede, async)
    // solicite a execução de um código na thread principal
    public static void Enqueue(Action action)
    {
        // Lock garante segurança em ambiente multithread
        lock (_actions)
        {
            _actions.Enqueue(action);
        }
    }

    // =========================
    //   EXECUÇÃO NO MAIN THREAD
    // =========================
    void Update()
    {
        // Executa todas as ações pendentes a cada frame
        while (_actions.Count > 0)
        {
            _actions.Dequeue().Invoke();
        }
    }
}