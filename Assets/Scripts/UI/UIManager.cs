using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singoton<UIManager> {

    private Dictionary<View, UIBase> viewPool = new Dictionary<View, UIBase>();
    private Transform canvas;
    private Transform[] layer = new Transform[4];

    public delegate void UpdateFunc(float deltaTime);
    public UpdateFunc OnUpdateHandle;

    public override void Initlize()
    {
        canvas = GameObject.Find("Canvas").transform;
        for(int i = 0; i < canvas.transform.childCount; i++)
        {
            layer[i] = canvas.GetChild(i);
        }
        EventManager.Instance.addEventListener<float>(EventName.GameUpdate, Update);
    }

    public T ShowView<T>(View view, object param = null) where T: UIBase
    {
        UIBase viewBase = null;
        if(!viewPool.TryGetValue(view, out viewBase) || viewPool[view] == null)
        {
            UIBase obj = System.Activator.CreateInstance(typeof(T)) as UIBase;
            viewBase = obj;

            viewBase.InitView(TrackRoot(viewBase));
            viewPool.Add(view, obj);
            viewBase._view = view;
            SetActive(viewBase.Root, true);
            viewBase.OnInit();
        }

        if (!viewBase.isVisible)
        {
            OnUpdateHandle += viewBase.Update;
            if(SetActive(viewBase.Root, true))
            {
                viewBase.isVisible = true;
                viewBase.OnShow(param);
            }
            else
                Debug.LogErrorFormat("This view init failed! :{0}", view.ToString());
        }
        return viewBase == null ? null : viewBase as T;
    }

    public void HideView(View view)
    {
        UIBase viewBase = null;
        if(!viewPool.TryGetValue(view, out viewBase) || viewPool[view] == null)
        {
            return;
        }
        if (viewBase.isVisible)
        {
            viewBase.isVisible = false;
            viewBase.DoHide(() =>
            {
                viewBase.OnHide();
                OnUpdateHandle -= viewBase.Update;
            });

        }
    }

    private bool SetActive(GameObject root, bool active)
    {
        if (root == null)
            return false;
        else
        {
            root.SetActive(active);
            return true;
        }
    }

    private GameObject TrackRoot(UIBase viewBase)
    {
        Transform parent = layer[(int)viewBase.GetLayer()];
        if (!string.IsNullOrEmpty(viewBase.GetViewPath()))
        {
            string path = string.Format("{0}", viewBase.GetViewPath());
            //Object prefab = Resources.Load(path);//ResourcesManager
            Object prefab = ResourcesManager.Instance.LoadPrefab(path);
            GameObject obj = Object.Instantiate(prefab) as GameObject;

            obj.name = prefab.name;
            obj.transform.parent = parent;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            return obj;
        }
        else
            Debug.LogErrorFormat("Undefined _PrefabName(): {0}", viewBase._view.ToString());
        return null;
    }

    private void Update(float deltaTime)
    {
        if (OnUpdateHandle != null)
            OnUpdateHandle(deltaTime);
    }

    public override void UnInitlize()
    {
        EventManager.Instance.removeEventListener<float>(EventName.GameUpdate, Update);
    }
}
