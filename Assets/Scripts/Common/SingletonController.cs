using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonController : EventSingleton<SingletonController> {

    protected EventMgr globalDispatcher = EventMgr.Instance;

}
