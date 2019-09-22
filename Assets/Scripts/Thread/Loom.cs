using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Linq;

/// <summary>
/// 主线程执行类Loom.
/// Reference: https://blog.csdn.net/wlz1992614/article/details/52326881
/// </summary>
public class Loom : SingletonManager<Loom>
{
    public static int maxThreads = 8;
    static int numThreads;

    private List<Action> _actions = new List<Action>();
    public struct DelayedQueueItem
    {
        public float time;
        public Action action;
    }
    private List<DelayedQueueItem> _delayed = new List<DelayedQueueItem>();

    List<DelayedQueueItem> _instanceDelayed = new List<DelayedQueueItem>();

    public static void QueueOnMainThread(Action action)
    {
        QueueOnMainThread(action, 0f);
    }
    public static void QueueOnMainThread(Action action, float time)
    {
        //if (time != 0)
        if(!Mathf.Approximately(time, 0))
        {
            if (Instance != null)
            {
                lock (Instance._delayed)
                {
                    Instance._delayed.Add(new DelayedQueueItem { time = Time.time + time, action = action });
                }
            }
        }
        else
        {
            if (Instance != null)
            {
                lock (Instance._actions)
                {
                    Instance._actions.Add(action);
                }
            }
        }
    }

    public Thread RunAsync(Action a)
    {
        while (numThreads >= maxThreads)
        {
            Thread.Sleep(1);
        }
        Interlocked.Increment(ref numThreads);
        ThreadPool.QueueUserWorkItem(RunAction, a);
        return null;
    }

    private static void RunAction(object action)
    {
        try
        {
            ((Action)action)();
        }
        catch
        {
        }
        finally
        {
            Interlocked.Decrement(ref numThreads);
        }

    }


    void OnDisable()
    {
        if (_instance == this)
        {

            _instance = null;
        }
    }



    // Use this for initialization  
    void Start()
    {

    }

    List<Action> _instanceActions = new List<Action>();

    // Update is called once per frame  
    void Update()
    {
        lock (_actions)
        {
            _instanceActions.Clear();
            _instanceActions.AddRange(_actions);
            _actions.Clear();
        }
        foreach (var a in _instanceActions)
        {
            a();
        }
        lock (_delayed)
        {
            _instanceDelayed.Clear();
            _instanceDelayed.AddRange(_delayed.Where(d => d.time <= Time.time));
            foreach (var item in _instanceDelayed)
                _delayed.Remove(item);
        }
        foreach (var delayed in _instanceDelayed)
        {
            delayed.action();
        }



    }
}  
