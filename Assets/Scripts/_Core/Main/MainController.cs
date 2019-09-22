using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : SingletonController<MainController>
{
    private MainModel _model;

    public override void Initialize()
    {
        base.Initialize();
        _model = MainModel.Instance;
    }

    public override void UnInitialize()
    {
        base.UnInitialize();
    }
}
