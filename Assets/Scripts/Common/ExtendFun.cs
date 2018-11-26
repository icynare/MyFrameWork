using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtendFun {

	public static GameObject FindChild(this GameObject obj, string path)
    {
        return obj.transform.Find(path).gameObject;
    }

    public static T GetChildComponent<T>(this GameObject obj, string path)
    {
        return obj.FindChild(path).GetComponent<T>();
    }

}
