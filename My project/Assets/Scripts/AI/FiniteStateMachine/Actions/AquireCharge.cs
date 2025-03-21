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
            SetBull(aIBlackBoard);
        }
    }
    private void SetBull(AIBlackBoard aIBlackBoard)
    {
        //aIBlackBoard.chargeOver = false;
        aIBlackBoard.chargeLocation = aIBlackBoard.chaseTarget.transform.position;
    }

}
