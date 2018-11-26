using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : Singleton<TimeManager> {

    private LinkedList<Timer> m_TimerList = new LinkedList<Timer>();

    public override void Initialize()
    {

    }

    protected LinkedListNode<Timer> m_curTimer = null;

    public void Update(float deltaTime, float unscaledDeltaTime)
    {
        m_curTimer = m_TimerList.First;
        while(m_curTimer != null)
        {
            var cur_timer = m_curTimer;
            m_curTimer = m_curTimer.Next;

            try
            {
                if (!cur_timer.Value.Run(deltaTime, unscaledDeltaTime))
                {
                    if(m_TimerList == cur_timer.List)
                    {
                        m_TimerList.Remove(cur_timer);
                        cur_timer.Value.Dispose();
                    }
                }
            }
            catch(System.Exception ex)
            {
                Debug.LogError(ex);
                //回调对象被删除
                if(m_TimerList == cur_timer.List)
                {
                    m_TimerList.Remove(cur_timer);
                    cur_timer.Value.Dispose();
                }
            }
        }
    }

    public Timer AddOnceTimer(float duration, bool unScale, CallBack<object[]> handler, params object[] args)
    {
        return Internal_AddTimer(1, duration, unScale, handler, args);
    }

    public Timer AddCountTimer(float duration, bool unScale, CallBack<object[]> handler, uint count, params object[] args)
    {
        return Internal_AddTimer((int)count, duration, unScale, handler, args);
    }

    public Timer AddRepeatTimer(float duration, bool unScale, CallBack<object[]> handler, params object[] args)
    {
        return Internal_AddTimer(-1, duration, unScale, handler);
    }

    public void ResetTimer(Timer timer)
    {
        if (timer == null)
            return;
        timer.Reset();
    }

    public void RemoveTimer(Timer timer)
    {
        if (timer == null)
            return;
        m_TimerList.Remove(timer);
        timer.Dispose();
        timer = null;
    }

    private Timer CreateObj()
    {
        return new Timer();
    }

    private Timer Internal_AddTimer(int count, float duration, bool unScale, CallBack<object[]> handler, params object[] args)
    {
        if (duration < 0.0f)
            return null;
        Timer timer = CreateObj();
        if(timer == null)
            return null;
        timer.Initialize(count, duration, unScale, handler, args);
        m_TimerList.AddFirst(timer);
        return timer;
    }

    public bool IsRunning(Timer timer)
    {
        var timerNode = m_TimerList.First;
        while(timerNode != null)
        {
            var curTimerNode = timerNode;
            timerNode = timerNode.Next;

            if (curTimerNode.Value == timer)
                return true;
        }
        return false;
    }

}
