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
    public int chosenSide;
    public Text t_instruct;
    public Text t_feedback;
    private RLEnvironment rl_environment;
    public Text t_rewards;
    public Text t_trials;

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
        chosenSide = -1;
        game_mode = EGAMEMODES.E_PUNCHOUTGAME;
        rl_environment = GetComponent<RLEnvironment>();
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
        if (chosenSide >= 0)
        {
            //StartCoroutine(rl_environment.Act());
            //if (chosenSide )
            //{
                //PlayerController.instance.restoreMaxHealth();
                
            //}
            if (chosenSide == 0)
            {
                EnemyBehaviour enemy = EnemyController.instance.getEnemy(0);
                //enemy.moveMeshCurved(PlayerController.instance.pawn.transform.GetChild(1).transform.position);
                enemy.moveMeshBezierCurved(PlayerController.instance.pawn.transform.GetChild(1).transform.position, PlayerController.instance.pawn.transform.position);
                if (Vector3.Distance(enemy.transform.position, PlayerController.instance.pawn.transform.GetChild(1).transform.position) < 0.1f)
                {
                    enemy.currLerpTime = 0f;
                    //chooseDuration.resetTimer();
                    //nextRoundTimer.resetTimer();
                    //roundStarted = false;
                    EnemyController.instance.resetEnemy(0);
                    //chosenSide = 0;
                    //if (PlayerController.instance.checkStatus(true))
                    //{
                    //    game_mode = EGAMEMODES.E_DEATH;
                    //}
                    //else
                    //{
                    //    t_feedback.text = "Good job! You dodged it!";
                    //}
                }
            }
            else if (chosenSide == 1)
            {
                EnemyBehaviour enemy = EnemyController.instance.getEnemy(1);
                //enemy.moveMeshCurved(PlayerController.instance.pawn.transform.GetChild(2).transform.position);
                enemy.moveMeshBezierCurved(PlayerController.instance.pawn.transform.GetChild(2).transform.position, PlayerController.instance.pawn.transform.position, false);
                if (Vector3.Distance(enemy.transform.position, PlayerController.instance.pawn.transform.GetChild(2).transform.position) < 0.1f)
                {
                    enemy.currLerpTime = 0f;
                    //chooseDuration.resetTimer();
                    //nextRoundTimer.resetTimer();
                    //roundStarted = false;
                    EnemyController.instance.resetEnemy(1);
                    //chosenSide = 0;
                    //if (PlayerController.instance.checkStatus(true))
                    //{
                    //    game_mode = EGAMEMODES.E_DEATH;
                    //}
                    //else
                    //{
                    //    t_feedback.text = "Good job! You dodged it!";
                    //}
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

    /// <summary>
    /// Check if AI's action is correct
    /// </summary>
    /// <param name="act">The action that AI made</param>
    /// <param return="reward">Reward ranges from (-1, 1)</param>
    public int GetRewards(int act)
    {
        int reward;
        int n;
        if (PlayerController.instance.pawn.rotated < 0)
            n = 0;
        else if (PlayerController.instance.pawn.rotated > 0)
            n = (int)rl_environment.parameterlength - 1;
        else 
            n = act;

        //Debug.Log("i chose: " + n);
        //Debug.Log("he chose: " + act);
        if (n == act)
        {
            reward = 1;
        }
        else
            reward = -1;
        t_rewards.text = "Total rewards: " + rl_environment.totalRewards.ToString();
        t_trials.text = "Num trials: " + rl_environment.trial.ToString();
        switch (act)
        {
            case 0:
                chosenSide = 1;
                break;
            case 1:
                chosenSide = 0;
                break;
        }
        rl_environment.trial += 1;
        StartCoroutine(rl_environment.Act());
        //StartCoroutine(delaylearning());
        return reward;
    }

    IEnumerator delaylearning()
    {
        //chosenSide = -1;
        yield return new WaitForSeconds(0.5f);
        rl_environment.trial += 1;
        StartCoroutine(rl_environment.Act());
    }
}
