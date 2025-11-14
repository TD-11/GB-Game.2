using System;
using System.Collections.Generic;
using UnityEngine;

public class UnityMainThreadDispatcher : MonoBehaviour
{
    private static readonly Queue<Action> _actions = new Queue<Action>();

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