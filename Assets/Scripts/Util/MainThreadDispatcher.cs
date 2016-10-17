using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

class MainThreadDispatcher : MonoBehaviour
{
    static Queue jobQueue = System.Collections.Queue.Synchronized(new Queue());
    static MainThreadDispatcher instance;
    public static void Init()
    {
        if (instance == null)
        {
            GameObject container = new GameObject();
            container.name = "MainThreadDispatcherContainer";
            instance = container.AddComponent(typeof(MainThreadDispatcher)) as MainThreadDispatcher;
        }
    }

    public static void Queue(Action job)
    {
        jobQueue.Enqueue(job);
    }

    void Update()
    {
        while (jobQueue.Count > 0)
        {
            var job = (Action)jobQueue.Dequeue();
            job();
        }
    }
}
