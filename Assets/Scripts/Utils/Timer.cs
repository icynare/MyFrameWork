using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer {

    private int _count = 0;
    private float _duration = 0.0f;
    private float _leftTime = 0.0f;
    private CallBack<object[]> _callback = null;
    private object[] _args = null;
    private bool _unScale = false;

    public void Initialize(int count, float duration, bool unScale, CallBack<object[]> handler, params object[] args)
    {
        _count = count;
        _duration = duration;
        _unScale = unScale;
        _leftTime = duration;
        _callback = handler;
        _args = args;
    }

    public bool Run(float delta, float unScaleDelta)
    {
        if (_callback == null)
            return false;
        if (_unScale)
            _leftTime -= unScaleDelta;
        else
            _leftTime -= delta;
        if (_leftTime > 0.0f)
            return true;

        if (_count >= 0)
        {
            if((_count == 1) || (_count == 0))
            {
                _callback(_args);
                return false;
            }
            --_count;
        }

        _callback(_args);
        _leftTime += _duration;
        return true;
    }

    public void Reset()
    {
        _count = 0;
        _duration = 0.0f;
        _leftTime = 0.0f;
        _unScale = false;
        _callback = null;
        _args = null;
    }

    public void Dispose()
    {
        Reset();
    }

}
