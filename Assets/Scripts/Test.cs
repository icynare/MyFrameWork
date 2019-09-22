using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ResourceManager.Instance.Initialize();
        UIManager.Instance.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(50, 50, 100, 100), "First"))
        {
            UIManager.Instance.ShowViewAsync<FirstView>(true, null);
        }

        if (GUI.Button(new Rect(50, 150, 100, 100), "Second"))
        {
            UIManager.Instance.ShowViewAsync<SecondView>(false, null);
        }

        if (GUI.Button(new Rect(50, 250, 100, 100), "回退"))
        {
            UIManager.Instance.PopView();
        }
    }

}
