//#define DONT_USE_ASSET_BUNDLE

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if DONT_USE_ASSET_BUNDLE && UNITY_EDITOR

public partial class ResourceManager : Singleton<ResourceManager>
{
    public string RelativeResPath;
    private Dictionary<Type, string> _typeSuffixDic;

    public override void Initialize()
    {
        base.Initialize();
        RelativeResPath = Application.dataPath + "/StreamingAssetsTemp/";
        _typeSuffixDic = new Dictionary<Type, string>()
        {
            [typeof(GameObject)] = ".prefab",
            [typeof(Sprite)] = ".png",//此处默认Sprite格式为.png，若有jpg图片加载另行补充
            [typeof(AudioClip)] = ".mp3"//此处默认音频格式为.mps，若有其他格式另行补充
        };
    }

    public T LoadRes<T>(string path, string resName) where T:UnityEngine.Object
    {
        if (!_typeSuffixDic.ContainsKey(typeof(T)))
        {
            PrintNotFound("类型错误");
            return default;
        }

        string resPath = RelativeResPath + resName + _typeSuffixDic[typeof(T)];
        T res = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(resPath);
        if (res == null)
        {
            PrintNotFound(resPath);
        }

        return res;
    }

    public void LoadResAsync<T>(string path, string resName, DataType.Callback<T> callback = null) where T:UnityEngine.Object
    {
        if (!_typeSuffixDic.ContainsKey(typeof(T)))
        {
            PrintNotFound("类型错误");
            callback?.Invoke(null);
            return;
        }

        string resPath = RelativeResPath + resName + _typeSuffixDic[typeof(T)];
        T res = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(resPath);
        if (res == null)
        {
            PrintNotFound(resPath);
        }
        callback?.Invoke(res);

    }

    private void PrintNotFound(string str)
    {
        Debug.LogError("[Resource Not Full]:" + str);
    }

}

#endif
