using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstView : ViewBase
{
    public override string GetViewPath()
    {
        return "FirstView";
    }

    public override UILayer GetViewLayer()
    {
        return UILayer.Normal;
    }

    public override void OnInit(string viewName, GameObject obj)
    {
        base.OnInit(viewName, obj);
        IsPermanent = true;
    }
}
