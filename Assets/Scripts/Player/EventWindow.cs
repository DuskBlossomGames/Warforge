using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventWindow
{
    public uint duration { get; private set; }
    public uint time { get; private set; }
    public bool isActive { get; private set; }
    public bool hasEnded { get; private set; }
    public EventWindow(uint maxDuration, bool startActive = true)
    {
        //AutoTick();
        duration = maxDuration;
        time = startActive ? duration : 0;

        if (time != 0) {
            isActive = true;
            hasEnded = false;
        } else
        {
            isActive = false;
            hasEnded = true;
        }
    }

    public void Tick()
    {
        //Debug.LogFormat("Tick from value {0}", time);
        if (time > 0)
        {
            time -= 1;
            isActive = true;
            hasEnded = false;
        } else
        {
            isActive = false;
            hasEnded = true;
        }
    }

    public void AutoTick()
    {
        Camera.main.GetComponent<MonoBehaviour>().StartCoroutine(AutoTicker());
    }

    public IEnumerator AutoTicker()
    {
        while (true)
        {
            Tick();
            yield return new WaitForFixedUpdate();
        }
    }

    public void Restart()
    {
        time = duration;
        if (time > 0)
        {
            isActive = true;
            hasEnded = false;
        }
        else
        {
            isActive = false;
            hasEnded = true;
        }
    }

    public void RestartAt(uint maxDuration)
    {
        duration = maxDuration;
        time = duration;
        if (time > 0)
        {
            isActive = true;
            hasEnded = false;
        }
        else
        {
            isActive = false;
            hasEnded = true;
        }
    }

    public void Zero()
    {
        time = 0;
    }

    public float Percent()
    {
        return time / (float)duration;
    }
}
