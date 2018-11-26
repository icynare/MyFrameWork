using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSingleton<T> : EventDispatcher {

    private static readonly object _lock = new object();
    private static T _instance;
    public static T Instance
    {
        get
        {
            if(_instance == null)
            {
                lock(_lock)
                {
                    _instance = (T)System.Activator.CreateInstance(typeof(T));
                }
            }
            return _instance;
        }
    }

    public virtual void Initialize()
    {

    }

    public virtual void UnInitialize()
    {

    }

}
