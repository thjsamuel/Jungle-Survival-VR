using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class FiniteStateMachine : RAINAction
{
    private AnimalBehaviour m_animalRef;
    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
        m_animalRef = ai.GetCustomElement<AnimalBehaviour>();
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        switch (m_animalRef.its_state)
        {
            case AnimalBehaviour.ANIMAL_STATE.RUN:
                {
                    if (ai.WorkingMemory.GetItem<GameObject>("attackRef") != null)
                    {
                        m_animalRef.its_state = AnimalBehaviour.ANIMAL_STATE.ATTACK;
                        //ai.WorkingMemory.SetItem("CanAttackPlayer", true);

                        return ActionResult.SUCCESS;
                    }
                }
                break;
            case AnimalBehaviour.ANIMAL_STATE.ATTACK:
                {
                    if (ai.WorkingMemory.GetItem<GameObject>("attackRef") == null)
                    {
                        //ai.WorkingMemory.SetItem("CanAttackPlayer", false);
                        return ActionResult.SUCCESS;
                    }
                }
                break;
        }
        return ActionResult.FAILURE;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}