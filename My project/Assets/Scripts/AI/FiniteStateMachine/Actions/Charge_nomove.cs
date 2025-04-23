using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[CreateAssetMenu(menuName = "FSM/AI/Actions/Charge_noMove", fileName = "A_StandStill")]

public class Charge_nomove : Action
{
    public override void Act(Blackboard blackboard)
    {
        if(blackboard is AIBlackBoard aiBlackBoard)
        {
            //aiBlackBoard.navMeshAgent.GetComponent<NavMeshAgent>().enabled = true;

            aiBlackBoard.navMeshAgent.speed = 0;
            aiBlackBoard.navMeshAgent.isStopped = true;
            aiBlackBoard.enemyAnimationController.chargeBreathing();
        }
    }
}
