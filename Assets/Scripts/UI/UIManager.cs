using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommonFramework.UI
{
    /// <summary>
    /// UI界面管理器，负责显示隐藏(创建/销毁)UI界面
    /// </summary>
    public class UIManager : SingletonManager<UIManager>
    {
        public const int LAYER_COUNT = 4;//UI层级数
        public const string VIEW_PATH = "View";//View资源加载的根目录

        protected Transform _canvas;
        protected Transform[] _layers;
        protected GameObject _objShield;

        protected Dictionary<string, ViewBase> _viewDic;
        protected Stack<ViewBase> _viewStack;

        public override void Initialize()
        {
            base.Initialize();
            _canvas = GameObject.Find("Canvas").transform;
            _layers = new Transform[LAYER_COUNT];
            for (int i = 0; i < _canvas.transform.childCount; i++)
            {
                _layers[i] = _canvas.GetChild(i);
            }
            _objShield = _layers[LAYER_COUNT - 1].transform.Find("Shield").gameObject;
            _viewDic = new Dictionary<string, ViewBase>();
            _viewStack = new Stack<ViewBase>();
            ScreenAdaption();
        }

        public override void UnInitialize()
        {
            base.UnInitialize();
        }

        /// <summary>
        /// 屏幕适配
        /// </summary>
        private void ScreenAdaption()
        {

        }

        /// <summary>
        /// 显示界面
        /// </summary>
        /// <typeparam name="T">界面类</typeparam>
        /// <param name="needHideCurView">是否需要隐藏/销毁当前窗口</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public ViewBase ShowView<T>(bool needHideCurView, params object[] args) where T : ViewBase
        {
            Type type = typeof(T);
            ViewBase view = null;
            string viewName = type.ToString();
            if (!_viewDic.TryGetValue(viewName, out view))
            {
                view = Activator.CreateInstance<T>();
                GenerateViewObj(viewName.ToString(), view);
                _viewDic.Add(viewName, view);
            }
            view = _viewDic[viewName];
            if (view.IsDestroyed)
            {
                GenerateViewObj(viewName.ToString(), view);
            }
            ShowView(view, needHideCurView, args);
            return view;
        }

        private void ShowView(ViewBase view, bool needHideCurView, params object[] args)
        {
            _objShield.SetActive(true);
            view.EnterFinishCallback = () =>
            {
                _objShield.SetActive(false);
                if (needHideCurView && _viewStack.Count > 0)
                {
                    PopView(false, false);
                }
                _viewStack.Push(view);
            };
            view.IsDestroyed = false;
            view.IsActive = true;
            view.Root.transform.SetAsLastSibling();
            view.OnShow(args);
        }

        /// <summary>
        /// 关闭界面
        /// </summary>
        /// <param name="needPop">是否弹出界面栈，默认弹出</param>
        /// <param name="showLastView">是否显示上一个界面，默认显示</param>
        public void PopView(bool needPop = true, bool showLastView = true)
        {
            if (_viewStack.Count < 1)
            {
                return;
            }

            ViewBase view = needPop ? _viewStack.Pop() : _viewStack.Peek();
            _objShield.SetActive(true);
            view.ExitFinishCallback = () =>
            {
                _objShield.SetActive(false);
                view.IsActive = false;
                if (!view.IsPermanent)
                {
                    view.IsDestroyed = true;
                    Destroy(view.Root);
                }

                if (showLastView && _viewStack.Count > 0)
                {
                    ViewBase lastView = _viewStack.Peek();
                    if (lastView.IsActive)
                    {
                        return;
                    }

                    if (lastView.IsDestroyed)
                    {
                        GenerateViewObj(lastView.ViewName, lastView);
                    }

                    lastView.IsActive = true;
                }
                
            };
            view.OnHide();
        }

        /// <summary>
        /// 关闭所有界面
        /// </summary>
        public void PopAllView()
        {
            while (_viewStack.Count > 0)
            {
                PopView(true, false);
            }
        }

        /// <summary>
        /// 生成View的GameObject模型
        /// </summary>
        private GameObject GenerateViewObj(string viewName, ViewBase view)
        {
            GameObject prefab = Resources.Load<GameObject>($"{VIEW_PATH}/{view.GetViewPath()}");//TODO 加载资源
            return InitViewObj(prefab, viewName, view);
        }

        public void ShowViewAsync<T>(bool needHideCurView, System.Action callback, params object[] args) where T : ViewBase
        {
            StartCoroutine(ShowViewEnumerator<T>(needHideCurView, callback, args));
        }

        private IEnumerator ShowViewEnumerator<T>(bool needHideCurView, System.Action callback, params object[] args)
            where T : ViewBase
        {
            Type type = typeof(T);
            ViewBase view = null;
            string viewName = type.ToString();
            if (!_viewDic.TryGetValue(viewName, out view))
            {
                view = Activator.CreateInstance<T>();
                yield return GenerateViewObjAsync(viewName.ToString(), view);
                _viewDic.Add(viewName, view);
            }
            view = _viewDic[viewName];
            if (view.IsDestroyed)
            {
                yield return GenerateViewObjAsync(viewName.ToString(), view);
            }

            ShowView(view, needHideCurView, args);
            callback?.Invoke();
        }

        private IEnumerator GenerateViewObjAsync(string viewName, ViewBase view)
        {
            ResourceRequest request = Resources.LoadAsync<GameObject>($"{VIEW_PATH}/{view.GetViewPath()}");//TODO 异步加载资源
            yield return request;
            GameObject prefab = request.asset as GameObject;
            InitViewObj(prefab, viewName, view);
        }

        private GameObject InitViewObj(GameObject prefab, string viewName, ViewBase view)
        {
            GameObject viewObj = Instantiate(prefab, Vector3.zero, Quaternion.Euler(Vector3.zero),
                _layers[(int)view.GetViewLayer()]);
            RectTransform rect = viewObj.GetComponent<RectTransform>();
            rect.offsetMax = Vector2.zero;
            rect.offsetMin = Vector2.zero;

            view.OnInit(viewName, viewObj);
            return viewObj;
        }
    }

}

