using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace CommonFramework.UI
{
    public abstract class ViewBase
    {
        //View绑定的GameObject
        public GameObject Root { get; private set; }
        //是否永驻界面（不会被销毁，只会隐藏）
        public bool IsPermanent { get; protected set; }
        //显示动画结束回调
        public AnimationCallback EnterFinishCallback;
        //隐藏动画结束回调
        public AnimationCallback ExitFinishCallback;

        //是否显示中
        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                Root.SetActive(value);
            }
        }

        //是否被销毁了
        public bool IsDestroyed;
        //View名称
        public string ViewName;
        //是否有进入动画
        protected bool _hasEnterAnim;
        //是否有退出动画
        protected bool _hasExitAnim;

        /// <summary>
        /// 返回ViewPrefab的相对地址
        /// </summary>
        public abstract string GetViewPath();

        /// <summary>
        /// 返回View所处的UI层级
        /// </summary>
        /// <returns></returns>
        public abstract UILayer GetViewLayer();

        /// <summary>
        /// View初始化，仅在每次生成GameObject时调用
        /// </summary>
        /// <param name="viewName">View名称</param>
        /// <param name="obj">View绑定的GameObject</param>
        public virtual void OnInit(string viewName, GameObject obj)
        {
            Root = obj;
            Root.name = viewName;
            ViewName = viewName;
        }

        /// <summary>
        /// 每次显示界面时调用，首次显示也会调用
        /// </summary>
        /// <param name="args">参数</param>
        public virtual void OnShow(params object[] args)
        {
            if (_hasEnterAnim)
            {
                PlayEnterAnimation();
            }
            else
            {
                EnterFinishCallback?.Invoke();
                OnEnterAnimEnd();
            }
        }

        /// <summary>
        /// 每次隐藏/销毁界面时调用
        /// </summary>
        /// <param name="args"></param>
        public virtual void OnHide(params object[] args)
        {
            if (_hasExitAnim)
            {
                PlayExitAnimation();
            }
            else
            {
                ExitFinishCallback?.Invoke();
                OnExitAnimEnd();
            }
        }

        /// <summary>
        /// 显示动画
        /// </summary>
        public virtual void PlayEnterAnimation()
        {
            //TEST 
            Root.transform.DOScale(1.2f * Vector3.one, 0.2f).OnComplete(() =>
            {
                Root.transform.DOScale(Vector3.one, 0.3f).OnComplete(() =>
                {
                    EnterFinishCallback?.Invoke();
                    OnEnterAnimEnd();
                });
            });
        }

        /// <summary>
        /// 退出动画
        /// </summary>
        public virtual void PlayExitAnimation()
        {
            //TEST
            Root.transform.DOScale(Vector3.zero, 0.5f).OnComplete(() =>
            {
                ExitFinishCallback?.Invoke();
                Root.transform.localScale = Vector3.one;
                OnExitAnimEnd();
            });
        }

        /// <summary>
        /// 显示动画播放完毕回调
        /// </summary>
        public virtual void OnEnterAnimEnd()
        {

        }

        /// <summary>
        /// 退出动画播放完毕回调
        /// </summary>
        public virtual void OnExitAnimEnd()
        {

        }

    }

}

