using System;
using System.Collections.Generic;
using UnityEngine;

public class EventDispatcher {

    private Dictionary<ValueType, Delegate> eventDic;

    //添加监听
    public void addEventListener(ValueType vtype, CallBack method)
    {
        if (!recordEvent(vtype, method)) return;
        eventDic[vtype] = Delegate.Combine((CallBack)eventDic[vtype], method);
    }

    public void addEventListener<T>(ValueType vtype, CallBack<T> method)
    {
        if (!recordEvent(vtype, method)) return;
        eventDic[vtype] = Delegate.Combine((CallBack<T>)eventDic[vtype], method);
    }

    //删除监听
    public void removeEventListener(ValueType vtype, CallBack method)
    {
        if (!removeEvent(vtype, method)) return;
        eventDic[vtype] = Delegate.Remove(eventDic[vtype], method);
        removeType(vtype);
    }

    public void removeEventListener<T>(ValueType vtype, CallBack<T> method)
    {
        if (!removeEvent(vtype, method)) return;
        eventDic[vtype] = Delegate.Remove(eventDic[vtype], method);
        removeType(vtype);
    }

    public void dispatchEvnet(ValueType vtype)
    {
        if (eventDic == null)
            return;
        if (!eventDic.ContainsKey(vtype))
            return;
        Delegate del = eventDic[vtype];
        if (del == null)
            return;
        CallBack callback = del as CallBack;
        if (callback != null)
        {
            callback();
        }
    }

    public void dispatchEvent<T>(ValueType vtype, T arg)
    {
        if (eventDic == null)
            return;
        if (!eventDic.ContainsKey(vtype))
            return;
        Delegate del = eventDic[vtype];
        if (del == null)
            return;
        CallBack<T> callback = del as CallBack<T>;
        if (callback != null)
        {
            callback(arg);
        }
    }

    private bool recordEvent(ValueType vtype, Delegate method)
    {
        if (eventDic == null)
            eventDic = new Dictionary<ValueType, Delegate>();

        if(!eventDic.ContainsKey(vtype))
        {
            eventDic.Add(vtype, null);
        }
        Delegate del = eventDic[vtype];
        eventDic.TryGetValue(vtype, out del);
        if(del != null)
        {
            if (del.GetType() != method.GetType())
                return false;
            eventDic[vtype] = Delegate.Remove(eventDic[vtype], method);
        }
        return true;
    }

    private bool removeEvent(ValueType vtype, Delegate method)
    {
        if (eventDic == null) return false;
        if (!eventDic.ContainsKey(vtype))
            return false;
        Delegate del = eventDic[vtype];
        if (del == null)
            return false;
        else if (del.GetType() != method.GetType())
            return false;
        else
            return true;
    }

    private void removeType(ValueType vtype)
    {
        if (eventDic == null)
            return;
        if (!eventDic.ContainsKey(vtype))
            return;
        if (eventDic[vtype] == null)
            eventDic.Remove(vtype);
    }
}
