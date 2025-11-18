using System;
using System.Collections.Generic;
using UnityEngine;

public class UnityMainThreadDispatcher : MonoBehaviour
{
    public static UnityMainThreadDispatcher Instance;

    private static readonly Queue<Action> _actions = new Queue<Action>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static void Enqueue(Action action)
    {
        lock (_actions)
        {
            _actions.Enqueue(action);
        }
    }

    void Update()
    {
        while (_actions.Count > 0)
        {
            _actions.Dequeue().Invoke();
        }
    }
}