using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour{
    public bool u_hasStarted;
    public bool u_runAction;
    public int u_secondsPassed;
    public int u_MAX_SECS;

    void Start () {
        u_hasStarted = false;
        u_secondsPassed = 0;
        u_runAction = false;
    }

    void Update () {

    }

    // Constructor
    public void initTimer(int MAX_SECONDS)
    {
        u_MAX_SECS = MAX_SECONDS;
    }

    /// <summary>
    /// Coroutine template
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartCR()
    {
        while(u_hasStarted)
        {
            ++u_secondsPassed;
            //Debug.Log(secondsPassed);

            if (u_secondsPassed == u_MAX_SECS)
            {
                u_runAction = true;
                stopTiming();
            }
            yield return new WaitForSeconds(1); // Called again after wait time
        }
    }

    public void startTiming()
    {
        if (!u_hasStarted)
        {
            u_hasStarted = true;
            StartCoroutine(StartCR());
        }
        else
            u_secondsPassed = 0;
    }

    public void startTimingNoRefresh()
    {
        if (!u_hasStarted)
        {
            u_hasStarted = true;
            StartCoroutine(StartCR());
        }
    }

    public void stopTiming()
    {
        u_hasStarted = false;
        u_secondsPassed = 0;
        StopCoroutine(StartCR());
    }

    public void resetTimer()
    {
        u_runAction = false;
    }
}
