using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINDecision]
public class animalFSM : RAINDecision
{
    private int _lastRunning = 0;
    private AnimalBehaviour m_animalBehave;

    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);

        _lastRunning = 0;
        m_animalBehave = ai.GetCustomElement<AnimalBehaviour>();
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        ActionResult tResult = ActionResult.SUCCESS;

        for (; _lastRunning < _children.Count; _lastRunning++)
        {
            tResult = _children[_lastRunning].Run(ai);
            if (tResult != ActionResult.SUCCESS)
                break;
        }

        stateMachineRun(ai);
        return tResult;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }

    public void stateMachineRun(RAIN.Core.AI ai)
    {
        switch (m_animalBehave.its_state)
        {
            case AnimalBehaviour.ANIMAL_STATE.PATROL:
                {
                    if (ai.WorkingMemory.GetItem<GameObject>("playerRef") != null)
                    {
                        // conduct distance check within staring distance
                        if (m_animalBehave.aiViewOfPlayer("EyeSight"))
                        if (!m_animalBehave.checkCloseEnough(m_animalBehave.m_stareDistance))
                        {
                            m_animalBehave.its_state = AnimalBehaviour.ANIMAL_STATE.INVESTIGATE;
                            Vector3 playerspos = m_animalBehave.player.transform.position;
                            ai.WorkingMemory.SetItem("lastSeenPos", playerspos);
                            ai.WorkingMemory.SetItem("CanInvestigate", true);                            
                        }
                        else
                        {
                            m_animalBehave.its_state = AnimalBehaviour.ANIMAL_STATE.STARE;
                            ai.WorkingMemory.SetItem("CanInvestigate", false); 
                        }
                    }
                }
                break;
            case AnimalBehaviour.ANIMAL_STATE.INVESTIGATE:
                {
                    if (m_animalBehave.checkCloseEnough(m_animalBehave.m_stareDistance))
                    {
                        Debug.Log("df");
                        m_animalBehave.its_state = AnimalBehaviour.ANIMAL_STATE.STARE;
                        ai.WorkingMemory.SetItem("CanInvestigate", false);
                    }
                    else if (m_animalBehave.checkCloseEnough(3, ai.WorkingMemory.GetItem<Vector3>("lastSeenPos")))
                    {
                        m_animalBehave.its_state = AnimalBehaviour.ANIMAL_STATE.PATROL;
                        ai.WorkingMemory.SetItem("CanInvestigate", false);
                        Debug.Log("finish ptrol");
                    }

                    //if (ai.WorkingMemory.GetItem<GameObject>("playerRef") == null)

                }
                break;
            case AnimalBehaviour.ANIMAL_STATE.STARE:
                {
                }
                break;
        }
    }
}