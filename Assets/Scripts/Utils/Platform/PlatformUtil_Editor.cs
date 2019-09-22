using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
public static partial class PlatformUtil
{
    public static string GetStreamingAssetsPath()
    {
        return $"{Application.dataPath}/{GameContants.TempStreamingAssetPath}/Editor/";
    }
}
#endif