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

    public enum EGAMEMODES
    {
        E_GUESSGAME,
        E_PUNCHOUTGAME,
        E_DEATH
    }
    public EGAMEMODES game_mode;

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
        chosenSide = 0;
        game_mode = EGAMEMODES.E_PUNCHOUTGAME;
	}
	
	// Update is called once per frame
	void Update () {
        /// First execution ///
        switch (game_mode)
        { 
            case EGAMEMODES.E_GUESSGAME:
               startGame("Round Starting! Click panel to join.");
        /// 3rd execution ///
        if (chooseDuration.u_runAction)
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
            t_instruct.text = chooseDuration.u_secondsPassed.ToString();
        }
        break;
            case EGAMEMODES.E_PUNCHOUTGAME:
        /// 3rd execution ///
        if (!roundStarted)
        {
            startGame("Round Can Start. Click panel to join!");
        }
        if (chooseDuration.u_runAction)
        {
            if (chosenSide == 0)
            {
                PlayerController.instance.restoreMaxHealth();

                int randVal = Random.Range(0, 100);
                if (randVal <= 49)
                {
                    chosenSide = 1;
                }
                else
                    chosenSide = -1;
            }

            if (Mathf.Sign(chosenSide) == 1)
            {
                EnemyBehaviour enemy = EnemyController.instance.getEnemy(0);
                //enemy.moveMeshCurved(PlayerController.instance.pawn.transform.GetChild(1).transform.position);
                enemy.moveMeshBezierCurved(PlayerController.instance.pawn.transform.GetChild(1).transform.position, PlayerController.instance.pawn.transform.position);
                if (Vector3.Distance(enemy.transform.position, PlayerController.instance.pawn.transform.GetChild(1).transform.position) < 0.1f)
                {
                    enemy.currLerpTime = 0f;
                    chooseDuration.resetTimer();
                    nextRoundTimer.resetTimer();
                    roundStarted = false;
                    EnemyController.instance.resetEnemy(0);
                    chosenSide = 0;
                    if (PlayerController.instance.checkStatus(true))
                    {
                        game_mode = EGAMEMODES.E_DEATH;
                    }
                    else
                    {
                        t_feedback.text = "Good job! You dodged it!";
                    }
                }
            }
            else if (Mathf.Sign(chosenSide) == -1)
            {
                EnemyBehaviour enemy = EnemyController.instance.getEnemy(1);
                //enemy.moveMeshCurved(PlayerController.instance.pawn.transform.GetChild(2).transform.position);
                enemy.moveMeshBezierCurved(PlayerController.instance.pawn.transform.GetChild(2).transform.position, PlayerController.instance.pawn.transform.position, false);
                if (Vector3.Distance(enemy.transform.position, PlayerController.instance.pawn.transform.GetChild(2).transform.position) < 0.1f)
                {
                    enemy.currLerpTime = 0f;
                    chooseDuration.resetTimer();
                    nextRoundTimer.resetTimer();
                    roundStarted = false;
                    EnemyController.instance.resetEnemy(1);
                    chosenSide = 0;
                    if (PlayerController.instance.checkStatus(true))
                    {
                        game_mode = EGAMEMODES.E_DEATH;
                    }
                    else
                    {
                        t_feedback.text = "Good job! You dodged it!";
                    }
                }
            }
        }
        else
        {
            t_instruct.text = chooseDuration.u_secondsPassed.ToString();
        }

        break;
            default:
                if (endGame("You died. Click on panel to try again after 3 seconds."))
                {
                    game_mode = EGAMEMODES.E_PUNCHOUTGAME;
                }
                t_instruct.text = nextRoundTimer.u_secondsPassed.ToString();

        break;
    }
	}

    public void startRound()
    {
        /// 2nd execution ///
        if (roundStarted)
        {
            t_feedback.text = "Round Started, Get Ready!";
            chooseDuration.startTimingNoRefresh();
        }
    }

    private bool startGame(string displayTxt)
    {
        if (!roundStarted)
        {
            nextRoundTimer.startTimingNoRefresh();
            if (nextRoundTimer.u_runAction)
            {
                roundStarted = true;
                t_feedback.text = displayTxt;
                return true; // round reset
            }
        }
        return false;
    }

    private bool endGame(string displayTxt)
    {
        t_feedback.text = displayTxt;
        if (!roundStarted)
        {
            nextRoundTimer.startTimingNoRefresh();
            if (nextRoundTimer.u_runAction)
            {
                return true; // round reset
            }
        }
        return false;
    }
}
