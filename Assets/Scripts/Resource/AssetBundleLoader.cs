using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AssetBundleLoader : Singleton<AssetBundleLoader>
{
    private Dictionary<string, AssetBundle> _loadedAssetBundles;

    public override void Initialize()
    {
        base.Initialize();
        _loadedAssetBundles = new Dictionary<string, AssetBundle>();
    }

    public AssetBundle LoadAssetBundle(string bundleName, string bundlePath)
    {
        if (_loadedAssetBundles.TryGetValue(bundleName, out AssetBundle ab))
        {
            return ab;
        }
        ab = AssetBundle.LoadFromFile(bundlePath);
        _loadedAssetBundles.Add(bundleName, ab);
        return ab;
    }

    public IEnumerator LoadAssetBundleAsync(string bundleName, string bundlePath, CallBack<AssetBundle> callback = null)
    {
        if (_loadedAssetBundles.TryGetValue(bundleName, out AssetBundle ab))
        {
            callback?.Invoke(ab);
            yield break;
        }
        AssetBundleCreateRequest assetBundleCreateRequest = AssetBundle.LoadFromFileAsync(bundlePath);
        yield return assetBundleCreateRequest;
        _loadedAssetBundles.Add(bundleName, assetBundleCreateRequest.assetBundle);
        callback?.Invoke(assetBundleCreateRequest.assetBundle);
    }

    public bool ReleaseAssetBundle(AssetBundle bundle)
    {
        if (_loadedAssetBundles.ContainsValue(bundle))
        {
            string bundleName = _loadedAssetBundles.FirstOrDefault(item => item.Value == bundle).Key;
            _loadedAssetBundles.Remove(bundleName);
            return true;
        }

        return false;
    }

    public bool ReleaseAssetBundle(string bundleName)
    {
        if (_loadedAssetBundles.ContainsKey(bundleName))
        {
            _loadedAssetBundles.Remove(bundleName);
            return true;
        }

        return false;
    }

}
