using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonController<T>: EventSingleton<T> {

    protected EventManager globalDispatcher = EventManager.Instance;
    protected ViewBase _view;

}
