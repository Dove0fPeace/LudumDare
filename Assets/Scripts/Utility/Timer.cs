using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    private static GameObject TimerCollector;
    
    public event UnityAction OnTimeRunOut;
    public event UnityAction OnTick;

    public bool IsLoop;

    public float MaxTime => maxTime;
    public float CurrentTime => currentTime;
    public bool IsPaused => isPaused;
    public bool IsCompleted => currentTime <= 0;

    private float maxTime;
    private float currentTime;

    private bool isPaused;

    private void Start()
    {
        currentTime = maxTime;
    }

    void Update()
    {
        if(isPaused) return;

        currentTime -= Time.deltaTime;
        OnTick?.Invoke();
        
        if (currentTime <= 0)
        {
            currentTime = 0;
            OnTimeRunOut?.Invoke();

            if (IsLoop)
            {
                currentTime = maxTime;
            }
        }
    }

    public static Timer CreateTimer(float time, bool isLoop)
    {
        if (TimerCollector == null)
        {
            TimerCollector = new GameObject("Timers");
        }

        Timer timer = TimerCollector.AddComponent<Timer>();

        timer.maxTime = time;
        timer.IsLoop = isLoop;

        return timer;
    }
    
    public static Timer CreateTimer(float time)
    {
        if (TimerCollector == null)
        {
            TimerCollector = new GameObject("Timers");
        }

        Timer timer = TimerCollector.AddComponent<Timer>();

        timer.maxTime = time;

        return timer;
    }

    public void Play()
    {
        isPaused = false;
    }

    public void Pause()
    {
        isPaused = true;
    }

    public void Completed()
    {
        isPaused = false;
        currentTime = 0;
    }

    public void Restart(float time)
    {
        maxTime = time;
        currentTime = maxTime;
    }

    public void Restart()
    {
        currentTime = maxTime;
    }

    public void Destroy()
    {
        Destroy(this);
    }
}
