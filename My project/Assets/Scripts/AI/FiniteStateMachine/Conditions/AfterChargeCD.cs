using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "FSM/AI/Actions/AfterChargeAFK", fileName = "A_AfterCharge")]

public class AfterChargeCD : Action
{
    public override void Act(Blackboard blackboard)
    {
        if (blackboard is AIBlackBoard aiBlackboard)
        {
            ChargeAFK(aiBlackboard);
        }
    }

    private void ChargeAFK(AIBlackBoard aiBlackboard) // change all of this to a condition later on but going to sleep now.
    {

    }


}
