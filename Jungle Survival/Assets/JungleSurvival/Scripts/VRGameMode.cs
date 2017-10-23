using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private RawImage[] ui_objects3d;
    public GameObject tutorialObj;
    private int tutorialStage;

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
        ui_objects3d = GameObject.Find("3DCanvas").GetComponentsInChildren<RawImage>(true);
        tutorialObj = GameObject.Find("2DCanvas").transform.GetChild(2).gameObject;
        tutorialStage = 1;
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
        //    activateSlowMotion();

        //if (PlayerController.instance.checkStatus(55))
            //pointsGained = maxpoint;

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

        if (e_gamestate == E_Gamestate.e_pause)
        {
            switch (tutorialStage)
            {
                case 1:
                    {
                        tutorialObj.SetActive(true);
                        //if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                             if (Input.GetMouseButton(0))
                            tutorialStage++;
                    }
                    break;
                case 2:
                    {
                        showTutorial2();
                    }
                    break;
                case 3:
                    {
                        showTutorial3();
                    }
                    break;
                case 4:
                    {
                        showTutorial4();
                    }
                    break;
                default:
                    tutorialObj.SetActive(false);
                    tutorialStage++;
                    break;
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

    public bool isRoundEnding()
    {
        if (!elapsedTime.u_runAction)
        {
            if (elapsedTime.u_secondsPassed > (elapsedTime.u_MAX_SECS * 0.75f))
            {
                return true;
            }
        }
        return false;
    }

    private void activateSlowMotion()
    {
        if (pointsGained >= maxpoint)
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
    
    private void showTutorial2()
    {
        Color co = tutorialObj.GetComponent<Image>().color;
        tutorialObj.GetComponent<Image>().color = new Color(co.r, co.g, co.b, 0.3f);
        tutorialObj.transform.GetChild(0).GetComponent<Text>().text = "Underneath the arrow \n is the enemy!";
        tutorialObj.transform.GetChild(1).GetComponent<Text>().text = "\n \nTilt your head \n to the arrow's side!";
        tutorialObj.transform.GetChild(2).gameObject.SetActive(false);
        if (PlayerController.instance.pawn.rotated < 0)
        {
            tutorialStage++;
        }
    }

    private void showTutorial3()
    {
        ui_objects3d[0].gameObject.SetActive(false);
        ui_objects3d[1].gameObject.SetActive(true);
        tutorialObj.transform.GetChild(1).GetComponent<Text>().text = "Tilt to the other side!";
        if (PlayerController.instance.pawn.rotated > 0)
        {
            ui_objects3d[1].gameObject.SetActive(false);
            tutorialObj.transform.GetChild(0).gameObject.SetActive(true);
            tutorialObj.transform.GetChild(2).gameObject.SetActive(true);
            tutorialObj.transform.GetChild(0).GetComponent<Text>().text = "Well done!";
            tutorialObj.transform.GetChild(1).GetComponent<Text>().text = "This is how you dodge";
            tutorialObj.transform.GetChild(2).GetComponent<Text>().text = "Dodge the enemies fists! \n Click to next tutorial";
            tutorialStage++;
        }
    }

    private void showTutorial4()
    {
        tutorialObj.transform.GetChild(1).GetComponent<Text>().text = "The green bar above your\n head is your health!";
        tutorialObj.transform.GetChild(2).GetComponent<Text>().text = "Watch out for a pink bar!\n When you see it, click\n to slow down time!";
        if (Input.GetMouseButton(0))
        //if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            tutorialStage++;
        }
    }
}
