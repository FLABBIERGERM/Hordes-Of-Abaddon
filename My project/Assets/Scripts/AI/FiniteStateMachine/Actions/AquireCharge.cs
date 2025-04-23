using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;


[CreateAssetMenu(menuName = "FSM/AI/Actions/AquireCharge", fileName = "A_AquireCharge")]

public class AquireCharge : Action
{
    public override void Act(Blackboard blackboard)
    {
        if (blackboard is AIBlackBoard aIBlackBoard)
        {
              aIBlackBoard.chargeLocation = aIBlackBoard.chaseTarget.position;
            
              Debug.Log("New Target Position is" +  aIBlackBoard.chargeLocation); // this should only go off 1 time so we shall see if its the issue.
            aIBlackBoard.owningController.GetComponent<Animator>()?.SetTrigger("Pre-Charging");
            aIBlackBoard.enemyAnimationController.chargeTauntAudio();
        }
    }
}
