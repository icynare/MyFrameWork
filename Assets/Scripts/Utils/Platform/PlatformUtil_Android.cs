using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if !UNITY_EDIOR && UNITY_ANDROID
public static partial class PlatformUil
{
    public static string GetStreamingAssetsPath()
    {
        return $"{Application.streamingAssetsPath}/Android/";
    }
}
#endif