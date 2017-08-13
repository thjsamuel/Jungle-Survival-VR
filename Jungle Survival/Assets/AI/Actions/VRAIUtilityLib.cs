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
                //if ()

                Debug.Log("succese");
                CloseEnough2Stare(ai, 5);
                //CloseEnough2Attack(ai, 2);
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
        switch (m_animalRef.its_state)
        {
            case AnimalBehaviour.ANIMAL_STATE.IDLE:
                {
                    if (m_animalRef.checkCloseEnough(dist) && m_animalRef.stare_behaviour.spotted)
                    {
                        if (ai.WorkingMemory.GetItem<bool>("CloseEnoughStare") == false)
                            ai.WorkingMemory.SetItem("CloseEnoughStare", true);
                        m_animalRef.its_state = AnimalBehaviour.ANIMAL_STATE.STARE;
                    }
                    else if (m_animalRef.checkCloseEnough(dist) && !m_animalRef.stare_behaviour.spotted)
                    {
                        Debug.Log("sneak attk!");
                        if (ai.WorkingMemory.GetItem<bool>("CanAttackPlayer") == false)
                            ai.WorkingMemory.SetItem("CanAttackPlayer", true);
                        m_animalRef.its_state = AnimalBehaviour.ANIMAL_STATE.RUN;
                    }
                }
                break;
            case AnimalBehaviour.ANIMAL_STATE.STARE:
                {
                    if (!PlayerController.instance.isStaringAtSomething)
                    {
                        m_animalRef.countToAttack.startTimingNoRefresh();
                        if (m_animalRef.countToAttack.runAction)
                        {
                            if (ai.WorkingMemory.GetItem<bool>("CanAttackPlayer") == false)
                                ai.WorkingMemory.SetItem("CanAttackPlayer", true);
                            m_animalRef.its_state = AnimalBehaviour.ANIMAL_STATE.RUN;

                            m_animalRef.countToAttack.runAction = false;
                        }
                    }
                    /*if (!PlayerController.instance.isStaringAtSomething)
                    if (ai.WorkingMemory.GetItem<bool>("CloseEnoughStare"))
                        ai.WorkingMemory.SetItem("CloseEnoughStare", false);
                    m_animalRef.its_state = AnimalBehaviour.ANIMAL_STATE.IDLE;
                    Debug.Log("you should attk");
                     
                     else
                     {
                     * 
                     * }
                     
                     */
                }
                break;
            case AnimalBehaviour.ANIMAL_STATE.RUN:

                break;
            default:

                break;
        }
        /*if (m_animalRef.checkCloseEnough(dist) && m_animalRef.stare_behaviour.spotted && m_animalRef.its_state != AnimalBehaviour.ANIMAL_STATE.STARE) // staring starts
        {
            if (ai.WorkingMemory.GetItem<bool>("CloseEnoughStare") == false)
                ai.WorkingMemory.SetItem("CloseEnoughStare", true);
            m_animalRef.its_state = AnimalBehaviour.ANIMAL_STATE.STARE;
        }
        else if (!m_animalRef.stare_behaviour.spotted && m_animalRef.its_state != AnimalBehaviour.ANIMAL_STATE.STARE)
        {
            
        }*/
        /*else if (m_animalRef.its_state == AnimalBehaviour.ANIMAL_STATE.STARE && !PlayerController.instance.isStaringAtSomething)
        {
            if (ai.WorkingMemory.GetItem<bool>("CloseEnoughStare"))
                ai.WorkingMemory.SetItem("CloseEnoughStare", false);
            m_animalRef.its_state = AnimalBehaviour.ANIMAL_STATE.IDLE;
            Debug.Log("you should attk");
        }
        else
        {
            Debug.Log("staring");
        }*/
        //else if (!PlayerController.instance.isStaringAtSomething)
        //{
            //if (ai.WorkingMemory.GetItem<bool>("CanAttackPlayer") != true)
                //ai.WorkingMemory.SetItem("CanAttackPlayer", true);
            //if (ai.WorkingMemory.GetItem<bool>("CloseEnoughStare") == true)
              //  ai.WorkingMemory.SetItem("CloseEnoughStare", false);
        //}
    }

    public void CloseEnough2Attack(RAIN.Core.AI ai, float dist)
    {
        if (m_animalRef.checkCloseEnough(dist)) // staring starts
        {
            if (ai.WorkingMemory.GetItem<bool>("CanAttackPlayer") != true)
                ai.WorkingMemory.SetItem("CanAttackPlayer", true);
            if (ai.WorkingMemory.GetItem<bool>("CloseEnoughStare") == true)
                ai.WorkingMemory.SetItem("CloseEnoughStare", false);
        }
        else if (ai.WorkingMemory.GetItem<bool>("CanAttackPlayer") == true)
        {
            ai.WorkingMemory.SetItem("CanAttackPlayer", false);
        }
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}