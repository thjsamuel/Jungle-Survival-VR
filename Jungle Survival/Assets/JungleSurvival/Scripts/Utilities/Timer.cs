using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour{
    public bool hasStarted;
    public bool runAction;
    public int secondsPassed;
    public int MAX_SECS;

    void Start () {
        hasStarted = false;
        secondsPassed = 0;
        runAction = false;
    }

    void Update () {

    }

    // Constructor
    public void initTimer(int MAX_SECONDS)
    {
        MAX_SECS = MAX_SECONDS;
    }

    /// <summary>
    /// Coroutine template
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartCR()
    {
        while(hasStarted)
        {
            ++secondsPassed;
            //Debug.Log(secondsPassed);

            if (secondsPassed == MAX_SECS)
            {
                runAction = true;
                stopTiming();
            }
            yield return new WaitForSeconds(1); // Called again after wait time
        }
    }

    public void startTiming()
    {
        if (!hasStarted)
        {
            hasStarted = true;
            StartCoroutine(StartCR());
        }
        else
            secondsPassed = 0;
    }

    public void startTimingNoRefresh()
    {
        if (!hasStarted)
        {
            hasStarted = true;
            StartCoroutine(StartCR());
        }
    }

    public void stopTiming()
    {
        hasStarted = false;
        secondsPassed = 0;
        StopCoroutine(StartCR());
    }

    public void resetTimer()
    {
        runAction = false;
    }
}
