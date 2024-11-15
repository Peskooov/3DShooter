using System;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    private static GameObject timerCollector;

    public event UnityAction OnTimeRanOut;
    public event UnityAction OnTick;

    public bool isLoop;

    public float MaxTime => maxTime;
    public float CurrentTime => currentTime;
    public bool IsPause => isPause;
    public bool IsCompleted => currentTime <= 0;

    private float maxTime;
    private float currentTime;
    private bool isPause;

    private void Update()
    {
        if (isPause) return;

        currentTime -= Time.deltaTime;

        OnTick?.Invoke();

        if (currentTime <= 0)
        {
            currentTime = 0;

            OnTimeRanOut?.Invoke();

            if (isLoop)
                currentTime = maxTime;
        }
    }

    public static Timer CreateTimer(float time, bool isLoop)
    {
        if (!timerCollector)
        {
            timerCollector = new GameObject("Timers");
        }

        Timer timer = timerCollector.AddComponent<Timer>();

        timer.maxTime = time;
        timer.isLoop = isLoop;

        return timer;
    }

    public static Timer CreateTimer(float time)
    {
        if (!timerCollector)
        {
            timerCollector = new GameObject("Timers");
        }

        Timer timer = timerCollector.AddComponent<Timer>();

        timer.maxTime = time;

        return timer;
    }

    public void Play()
    {
        isPause = false;
    }

    public void Pause()
    {
        isPause = true;
    }

    public void Copmlited()
    {
        isPause = false;

        currentTime = 0;
    }

    public void CopmlitedWithoutEvent()
    {
        Destroy(this);
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
}