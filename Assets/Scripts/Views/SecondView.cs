using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondView : ViewBase
{
    public override string GetViewPath()
    {
        return "SecondView";
    }

    public override UILayer GetViewLayer()
    {
        return UILayer.Normal;
    }

    public override void OnInit(string viewName, GameObject obj)
    {
        base.OnInit(viewName, obj);
        _hasEnterAnim = true;
        _hasExitAnim = true;
    }
}
