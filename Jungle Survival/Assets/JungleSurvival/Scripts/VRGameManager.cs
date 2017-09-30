using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRGameManager : MonoBehaviour {
    public static VRGameManager instance = null;
    // Timers
    private Timer nextRoundTimer;
    private Timer chooseDuration;
    // Gameplay code
    private bool roundStarted;
    private const int SIDE1 = 0;
    private const int SIDE2 = 1;
    private int chosenSide;
    public Text t_instruct;
    public Text t_feedback;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

	// Use this for initialization
	void Start () {
        nextRoundTimer = gameObject.AddComponent<Timer>();
        nextRoundTimer.initTimer(3);
        chooseDuration = gameObject.AddComponent<Timer>();
        chooseDuration.initTimer(5);
        chosenSide = -1;
	}
	
	// Update is called once per frame
	void Update () {
        /// First execution ///
        if (!roundStarted)
        {
            nextRoundTimer.startTimingNoRefresh();
            if (nextRoundTimer.runAction)
            {
                roundStarted = true;
                t_feedback.text = "Round Starting! Click panel to join.";
            }
        }
        /// 3rd execution ///
        if (chooseDuration.runAction)
        {
            Debug.Log("on");
            int randVal = Random.Range(0, 100);
            if (randVal <= 49)
            {
                chosenSide = 1;
            }
            else
                chosenSide = -1;
            if (PlayerController.instance.pawn.rotated != 0)
            {
                if (Mathf.Sign(chosenSide) == Mathf.Sign(PlayerController.instance.pawn.rotated))
                {
                    t_feedback.text = "You won!";
                }
                else
                    t_feedback.text = "You lost!";
            }
            else
                t_feedback.text = "You didn't tilt to play!";

            chooseDuration.resetTimer();
            nextRoundTimer.resetTimer();
            roundStarted = false;
        }
        else
        {
            t_instruct.text = chooseDuration.secondsPassed.ToString();
        }
	}

    public void startRound()
    {
        /// 2nd execution ///
        if (roundStarted)
        {
            Debug.Log("round start!");
            chooseDuration.startTimingNoRefresh();
        }
    }
}
