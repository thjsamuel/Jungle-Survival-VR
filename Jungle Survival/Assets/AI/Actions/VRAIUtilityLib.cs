using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using RAIN.Serialization;

[RAINDecision]
public class VRAIUtilityLib : RAINDecision
{
    private int _lastRunning = 0;
    private AnimalBehaviour m_animalRef;

    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
        _lastRunning = 0;

        m_animalRef = ai.GetCustomElement<AnimalBehaviour>();
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        ActionResult tResult = ActionResult.FAILURE;

        if (ai.WorkingMemory.GetItem<GameObject>("playerRef") != null) // if tiger gains sight of player, therefore not null
        {
            if (m_animalRef.aiViewOfPlayer("TigerEyeSight")) // if tiger's sight is not blocked by me
            {
                tResult = ActionResult.SUCCESS; // If animal basically has the player in sight and is not blocked, it has succeeded in seeing the player
                CloseEnough2Stare(ai, 5);
                ai.WorkingMemory.SetItem("isPlayerBlocked", false);
            }
            else if (!ai.WorkingMemory.GetItem<bool>("isPlayerBlocked"))
                ai.WorkingMemory.SetItem("isPlayerBlocked", true);
        }
        /*
    else
    {
        if (ai.WorkingMemory.GetItem<bool>("CanAttackPlayer") != true)
        {
            ai.WorkingMemory.SetItem("CanAttackPlayer", false);
            tResult = ActionResult.FAILURE;
        }
    }
             
        if (m_animalRef.checkCloseEnough(5))
            {
                if (ai.WorkingMemory.GetItem<bool>("CanAttackPlayer") != true)
                    ai.WorkingMemory.SetItem("CanAttackPlayer", true);
                tResult = ActionResult.SUCCESS;
            }
            else if (ai.WorkingMemory.GetItem<bool>("CanAttackPlayer") == true) ai.WorkingMemory.SetItem("CanAttackPlayer", false);
         * * */

        return tResult;
    }

    public void CloseEnough2Stare(RAIN.Core.AI ai, float dist)
    {
        if (m_animalRef.checkCloseEnough(dist) && (PlayerController.instance.isStaringAtSomething)) // staring starts
        {
                if (ai.WorkingMemory.GetItem<bool>("CloseEnoughStare") != true)
                    ai.WorkingMemory.SetItem("CloseEnoughStare", true);
        }
        else if (ai.WorkingMemory.GetItem<bool>("CloseEnoughStare") == true)
            ai.WorkingMemory.SetItem("CloseEnoughStare", false);

    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}