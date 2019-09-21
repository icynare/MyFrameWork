using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coroutine
{
    CoroutineManager.CoroutineState task;

    public bool Running
    {
        get
        {
            return task.Running;
        }
    }

    public bool Paused
    {
        get
        {
            return task.Paused;
        }
    }

    public delegate void FinishedHandler(bool manual);
    public event FinishedHandler Finished;

    public Coroutine(IEnumerator c, bool autoStart = true)
    {
        task = CoroutineManager.CreateTask(c);
        task.Finished += TaskFinished;
        if (autoStart)
            Start();
    }

    public void Start()
    {
        task.Start();
    }

    public void Pause()
    {
        task.Pause();
    }

    public void UnPause()
    {
        task.UnPause();
    }

    void TaskFinished(bool manual)
    {
        FinishedHandler handler = Finished;
        if (handler != null)
            handler(manual);
    }

}



public class CoroutineManager : SingletonManager<CoroutineManager> {

    public class CoroutineState
    {
        public bool Running
        {
            get
            {
                return running;
            }
        }

        public bool Paused
        {
            get
            {
                return paused;
            }
        }

        public delegate void FinishedHandler(bool manual);
        public event FinishedHandler Finished;

        IEnumerator coroutine;
        bool running;
        bool paused;
        bool stopped;

        public CoroutineState(IEnumerator c)
        {
            coroutine = c;
        }

        public void Pause()
        {
            paused = true;
        }

        public void UnPause()
        {
            paused = false;
        }

        public void Start()
        {
            running = true;
            if (Instance == null)
                Debug.Log("Instance is null");
            Instance.StartCoroutine(CallWrapper());
        }

        public void Stop()
        {
            stopped = true;
            running = false;
        }

        IEnumerator CallWrapper()
        {
            yield return null;
            IEnumerator e = coroutine;
            while (running)
            {
                if (paused)
                    yield return null;
                else
                {
                    if (e != null && e.MoveNext())
                        yield return e.Current;
                    else
                        running = false;
                }
            }

            FinishedHandler handler = Finished;
            if (handler != null)
                handler(stopped);
        }
    }

    public static CoroutineState CreateTask(IEnumerator coroutine)
    {
        return new CoroutineState(coroutine);
    }

    public static UnityEngine.Coroutine Start(IEnumerator coroutine)
    {
        return Instance.StartCoroutine(coroutine);
    }

}

delegate IEnumerator CoroutineFun();
delegate IEnumerator CoroutineFun<T>(T t);
delegate IEnumerator CoroutineFun<T, U>(T t, U u);
