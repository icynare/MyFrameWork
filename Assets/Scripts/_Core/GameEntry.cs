using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEntry : MonoBehaviour
{
    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        LogicSign.Instance.Initialize();
        ResourceManager.Instance.Initialize();
        EventManager.Instance.Initialize();
        UIManager.Instance.Initialize();
        Loom.Instance.Initialize();
        CoroutineManager.Instance.Initialize();
        TimeManager.Instance.Initialize();
        //to add...
    }

    private void UnInit()
    {
        //to add...
        TimeManager.Instance.UnInitialize();
        CoroutineManager.Instance.UnInitialize();
        Loom.Instance.UnInitialize();
        UIManager.Instance.UnInitialize();
        EventManager.Instance.UnInitialize();
        ResourceManager.Instance.UnInitialize();
        LogicSign.Instance.UnInitialize();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        UnInit();
    }
}
