using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainModel : Model<MainModel>
{
    private MainController _controller;

    public override void Initialize()
    {
        base.Initialize();
        _controller = MainController.Instance;
    }

}
