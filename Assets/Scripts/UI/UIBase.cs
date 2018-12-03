using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIBase {
    public View _view;
    public bool isVisible = false;
    private GameObject _root;


    public GameObject Root
    {
        get
        {
            return _root;
        }
    }

    public void InitView(GameObject root)
    {
        _root = root;
    }

    public virtual void OnInit()
    {

    }

    public virtual void OnShow(object param = null)
    {

    }

    public virtual void OnHide()
    {

    }

    public virtual void OnDestroy()
    {

    }

    public virtual void Update(float deltaTime)
    {

    }

    public virtual void DoHide(System.Action func)
    {
        if (_root)
            _root.SetActive(false);
        if (func != null)
            func();
    }

    public abstract string GetViewPath();
    public abstract Layer GetLayer();


}
