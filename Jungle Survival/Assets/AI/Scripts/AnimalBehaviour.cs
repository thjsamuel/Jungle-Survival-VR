﻿using RAIN.Core;
using RAIN.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RAINSerializableClass]
public class AnimalBehaviour : CustomAIElement
{
    public GameObject player;
    public StaringBehaviour stare_behaviour;
    public Timer countToAttack;
    public float m_stareDistance;
    public static RAIN.Core.AI airef;

    public enum ANIMAL_STATE
    {
        IDLE,
        PATROL,
        INVESTIGATE,
        STARE,
        RUN,
        ATTACK
    }

    public ANIMAL_STATE its_state;

    public override void AIInit()
    {
        base.AIInit();
        // This is equivilent to an Awake call
        stare_behaviour = AI.Body.GetComponent<StaringBehaviour>();
        its_state = ANIMAL_STATE.PATROL;
        countToAttack = AI.Body.AddComponent<Timer>();
        countToAttack.initTimer(5);
        m_stareDistance = 10;
        airef = AI;
    }

    public override void Pre()
    {
        base.Pre();

        // This is the very first step, usually at the beginning of the Update call
        // This happens right after the Motor calls UpdateMotionTransforms
    }

    public override void Sense()
    {
        base.Sense();

        // This happens right after the the Senses have updated any detected Aspects
    }

    public override void Think()
    {
        base.Think();

        // This happens right after the Mind has decided what actions to take
    }

    public override void Act()
    {
        base.Act();

        // This happens right after the Motor has applied any movement changes to the AI
    }

    public override void Post()
    {
        base.Post();

        // This is the last step, usually at the end of the LateUpdate call
        // This happens right after the Motor and Animator have updated animations
    }

    /// <summary>
    /// A simple distance check to see if the AI is close enough to player
    /// </summary>
    /// <returns>true if close enough</returns>
    public bool checkCloseEnough(float dist)
    {
        if (Vector3.Distance(AI.Body.transform.position, player.transform.position) < dist)
            return true;

        return false;
    }

    /// <summary>
    /// A simple distance check to see if the AI is close enough to specified position
    /// </summary>
    /// <returns>true if close enough</returns>
    public bool checkCloseEnough(float dist, Vector3 checkpos)
    {
        if (Vector3.Distance(AI.Body.transform.position, checkpos) < dist)
            return true;

        return false;
    }

    /// <summary>
    /// Is the AI's view of player being obstructed by an object?
    /// Called when AI has sighted player behind wall
    /// </summary>
    /// <returns>true if it is obstructed</returns>
    public bool aiViewOfPlayer(string sensorname)
    {
        Vector3 sensorPos = ((RAIN.Perception.Sensors.VisualSensor)AI.Senses.GetSensor(sensorname)).Position;
        Vector3 direction = (player.transform.position - sensorPos).normalized;
        //Vector3 direction = (AI.WorkingMemory.GetItem<Vector3>("lastSeenPos") - sensorPos).normalized;
        RaycastHit rayinfo;
        //LayerMask mask = 1 << 11;
        if (Physics.Raycast(sensorPos, direction, out rayinfo, ((RAIN.Perception.Sensors.VisualSensor)AI.Senses.GetSensor(sensorname)).Range /*, mask*/))
        {
            if (rayinfo.collider.gameObject.layer >= 11) // anything after OpaqueView is basically something that blocks ai sight
            {
                return false;
            }
        }
        else if (rayinfo.Equals(null))
        {
            return false;
        }
        //Debug.DrawRay(sensorPos, direction, Color.red, 100);

        return true;
    }
}