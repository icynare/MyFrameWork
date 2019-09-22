using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainView : UIBase
{
    public override UILayer GetLayer()
    {
        return UILayer.Normal;
    }

    public override string GetViewPath()
    {
        return "MainView";
    }
}
