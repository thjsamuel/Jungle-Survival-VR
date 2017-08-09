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

        if (ai.WorkingMemory.GetItem<GameObject>("playerRef") != null)
        {
            if (m_animalRef.aiViewOfPlayer("TigerEyeSight"))
            {
                if (ai.WorkingMemory.GetItem<bool>("isPlayerBlocked") == true)
                {
                    ai.WorkingMemory.SetItem("isPlayerBlocked", false);

                }
                if (m_animalRef.checkCloseEnough())
                {
                    if (ai.WorkingMemory.GetItem<bool>("CanAttackPlayer") != true)
                        ai.WorkingMemory.SetItem("CanAttackPlayer", true);
                    tResult = ActionResult.SUCCESS;
                }
            }
            else if (ai.WorkingMemory.GetItem<bool>("isPlayerBlocked") != true)
            {
                ai.WorkingMemory.SetItem("isPlayerBlocked", true);
            }
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
             * */

        return tResult;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }

}