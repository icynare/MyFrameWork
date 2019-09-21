using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonManager<T> : MonoBehaviour where T : MonoBehaviour
{

    protected static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = GameObject.Find("GameController");
                if (obj == null)
                {
                    obj = new GameObject();
                    DontDestroyOnLoad(obj);
                    obj.name = "GameController";
                }
                _instance = obj.GetComponent<T>();
                if (_instance == null)
                    _instance = obj.AddComponent<T>();
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
