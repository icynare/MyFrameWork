using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if !UNITY_EDITOR && UNITY_IOS
public static partial class PlatformUtil
{
    public static string GetStreamingAssetsPath()
    {
        return $"{Application.dataPath}/IOS/";
    }
}
#endif