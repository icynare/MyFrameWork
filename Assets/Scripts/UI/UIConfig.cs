using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommonFramework.UI
{
    /// <summary>
    /// UI层级
    /// </summary>
    public enum UILayer
    {
        Bottom, //背景层
        Normal, //普通界面放置层
        Top, //弹窗，提示窗放置层
        TopMost //屏蔽层，屏蔽点击等
    }

    public delegate void AnimationCallback();

}
