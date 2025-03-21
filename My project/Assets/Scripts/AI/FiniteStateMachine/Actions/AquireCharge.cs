using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "FSM/AI/Actions/AquireCharge", fileName = "A_AquireCharge")]

public class AquireCharge : Action
{
    public override void Act(Blackboard blackboard)
    {
        if (blackboard is AIBlackBoard aIBlackBoard)
        {
            aIBlackBoard.chargeOver = true;
            aIBlackBoard.chargeLocation = aIBlackBoard.chaseTarget.position;
        
        }
    }
}
