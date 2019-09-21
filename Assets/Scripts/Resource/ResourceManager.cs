#define USE_ASSET_BUNDLE
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if USE_ASSET_BUNDLE

public partial class ResourceManager : Singleton<ResourceManager>
{
    public string RelativeResPath;

    public override void Initialize()
    {
        base.Initialize();
        AssetBundleLoader.Instance.Initialize();
        RelativeResPath = Application.streamingAssetsPath + "/";
    }

    public T LoadRes<T>(string path, string assetBundleName, string resName) where T : UnityEngine.Object
    {
        string assetBundlePath = RelativeResPath + assetBundleName;
        AssetBundle bundle = AssetBundleLoader.Instance.LoadAssetBundle(assetBundleName, assetBundlePath);
        if (bundle == null)
        {
            PrintNotFound($"Bundle Not Found:{assetBundlePath}");
            return default;
        }

        T res = bundle.LoadAsset<T>(resName);
        if (res == null)
        {
            PrintNotFound($"Bundle:{assetBundlePath}__Asset:{resName}");
        }

        return res;
    }

    public IEnumerator LoadResAsync<T>(string path, string assetBundleName, string resName, DataType.Callback<T> callback = null) where T : UnityEngine.Object
    {
        string assetBundlePath = RelativeResPath + assetBundleName;
        AssetBundle ab = null;
        yield return AssetBundleLoader.Instance.LoadAssetBundleAsync(assetBundleName, assetBundlePath,
            bundle => { ab = bundle; });
        AssetBundleRequest assetBundleRequest = ab.LoadAssetAsync(resName);
        yield return assetBundleRequest;
        callback?.Invoke(assetBundleRequest.asset as T);
    }

    public T LoadResByAssetBundle<T>(AssetBundle assetBundle, string resName) where T : UnityEngine.Object
    {
        T res = assetBundle.LoadAsset<T>(resName);
        if (res == null)
        {
            PrintNotFound($"Bundle:{assetBundle}__Asset:{resName}");
        }
        return res;
    }

    public IEnumerator LoadResByAssetBundleAsync<T>(AssetBundle assetBundle, string resName,
        DataType.Callback<T> callback = null) where T : UnityEngine.Object
    {
        AssetBundleRequest assetBundleRequest = assetBundle.LoadAssetAsync(resName);
        yield return assetBundleRequest;
        callback?.Invoke(assetBundleRequest.asset as T);
    }

    private void PrintNotFound(string str)
    {
        Debug.LogError("[Resource Not Full]:" + str);
    }
}

#endif