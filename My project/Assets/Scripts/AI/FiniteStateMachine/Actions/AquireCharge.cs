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
              aIBlackBoard.chargeLocation = aIBlackBoard.chaseTarget.position;
              //Debug.Log("New Target Position is" +  aIBlackBoard.chargeLocation); // this part is working as intended so i commented it out.
        }
    }
}
