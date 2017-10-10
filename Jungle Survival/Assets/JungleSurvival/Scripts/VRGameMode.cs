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
    // in seconds
    private int gmduration;
    public int accDuration
    {
        get { return gmduration; }
        set { elapsedTime.initTimer(value); }
    }
	// Use this for initialization
	void Start () {
        elapsedTime = gameObject.AddComponent<Timer>();
        e_diff = E_Difficulty.e_easy;
        e_gamestate = E_Gamestate.e_inprogress;
	}
	
	// Update is called once per frame
	void Update () {
		
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
    }
}
