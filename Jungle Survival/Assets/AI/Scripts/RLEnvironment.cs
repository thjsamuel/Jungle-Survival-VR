﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RLEnvironment : MonoBehaviour {
    public float totalRewards; // Total rewards obtained over the course of all trials.
    public int trial; // Trial index.
    int num_combos; // Number of types of punches in a given trial.
    public float actSpeed; // Speed at which actions are chosen.
    float[] armProbs; // True values for each chests in each room.
    RLAgent agent; // The agent which learns to pick actions.
    public float parameterlength;
   
    /// <summary>
	/// Initialized the bandit game. Called when "Start Learning" button in clicked.
	/// </summary>
    public void BeginLearning()
    {
        trial = 0;
        totalRewards = 0;
        num_combos = 2;
        bool optimistic = true;//GameObject.Find("optToggle").GetComponent<Toggle>().isOn;

        agent = new RLAgent(num_combos, optimistic);
        parameterlength = agent.value_table.Length;
        StartCoroutine(Act());
    }

	// Use this for initialization
	void Start () {
        actSpeed = 0.5f;
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    Application.Quit();
        //}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Gets an action from the agent, chooses the punch accordingly,
    /// and updates the agent's value estimates based on the reward.
    /// </summary>
    public IEnumerator Act()
    {
        yield return new WaitForSeconds(actSpeed); // wait for actSpeed seconds
        int action = agent.PickAction();
        VRGameManager.instance.chosenSide = action;
        float reward = VRGameManager.instance.GetRewards(action);
        totalRewards += reward;
        agent.UpdatePolicy(action, reward);
    }
}
