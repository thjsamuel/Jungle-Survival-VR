using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRGameMode : MonoBehaviour {
    private Timer elapsedTime;
    public enum E_Difficulty
    {
        e_easy,
        e_medium,
        e_hard,
        e_pro
    }
    public enum E_Gamestate
    {
        e_inprogress,
        e_win,
        e_lose,
        e_pause
    }
    public E_Difficulty e_diff;
    public E_Gamestate e_gamestate;
    private Timer effectTimer;
    private int pointsGained;
    private const int maxpoint = 25;
    // in seconds
    private int gmduration;
    public int accDuration
    {
        get { return gmduration; }
        set { elapsedTime.initTimer(value); }
    }

    // Slowmotion stuff
    private float slowScale = 0.25f;
    private float normScale = 1.0f;
    private GameObject u_timeindicator; 

	// Use this for initialization
	void Start () {
        elapsedTime = gameObject.AddComponent<Timer>();
        e_diff = E_Difficulty.e_easy;
        e_gamestate = E_Gamestate.e_pause;
        effectTimer = gameObject.AddComponent<Timer>();
        effectTimer.initTimer(10);
        StartCoroutine( countBySecond());
        u_timeindicator = GameObject.Find("SlowmotionIndicator");
        u_timeindicator.SetActive(false);
        //pointsGainTime.startTimingNoRefresh();
	}
	
	// Update is called once per frame
	void Update () {
        //if (pointsGained >= maxpoint)
        //{
        //    pointsGainTime.resetTimer();
        //}
        //if (pointsGainTime.u_runAction)
        //{
        //    pointsGained++;
        //    Debug.Log(pointsGained);

        //    pointsGainTime.resetTimer();
        //    pointsGainTime.startTimingNoRefresh();
        //}
        if (Input.touchCount > 0 && (e_gamestate != E_Gamestate.e_win && e_gamestate != E_Gamestate.e_lose))
            if (Input.GetTouch(0).phase == TouchPhase.Began)
                activateSlowMotion();
        //if (Input.GetMouseButton(0) && (e_gamestate != E_Gamestate.e_win && e_gamestate != E_Gamestate.e_lose))
            //activateSlowMotion();

        if (pointsGained >= maxpoint)
        {
            if (u_timeindicator.activeSelf == false)
                u_timeindicator.SetActive(true);
            if (effectTimer.u_runAction)
            {
                if (resetSlowMotion())
                {
                    effectTimer.u_runAction = false;
                    pointsGained = 0;
                    StartCoroutine(countBySecond());
                    u_timeindicator.SetActive(false);
                }
            }
        }
	}

    public void raiseDifficulty(ref float speed, float rise)
    {
        speed -= rise;
        if (speed < 1)
            speed = 1;
    }

    public void startGameCountdown()
    {
        elapsedTime.startTimingNoRefresh();
    }

    public bool hasRoundEnded()
    {
        return elapsedTime.u_runAction;
    }

    public void endRound()
    {
        elapsedTime.resetTimer();
        elapsedTime.stopTiming();
        if (e_gamestate == E_Gamestate.e_lose)
            pointsGained = 0;
    }

    private void activateSlowMotion()
    {
        if (pointsGained >= maxpoint && PlayerController.instance.checkStatus(51))
        {
            if (!effectTimer.u_runAction)
            {
                Time.timeScale = slowScale;
                Time.fixedDeltaTime = Time.timeScale * 0.02f;
                effectTimer.startTimingNoRefresh();
            }
            //VRGameManager.instance.rl_environment.actSpeed *= Time.timeScale;
        }
    }

    private bool resetSlowMotion()
    {
        Time.timeScale += slowScale;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0, normScale);
        if (Time.timeScale >= normScale)
        {
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            return true;
        }
        return false;
    }

    private IEnumerator countBySecond()
    {
        pointsGained++;
        yield return new WaitForSeconds(1);
        if (pointsGained < maxpoint)
        {
            StartCoroutine(countBySecond());
        }
    }
}
