using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "FSM/AI/Conditions/ChargeOver_Condition", fileName = "C_CHO")]

public class ChargeOverCD : Condition
{
    public override bool Evaluate(Blackboard blackboard)
    {
        if (blackboard is AIBlackBoard aIblackboard)
        {
            if (aIblackboard.chargeOver) {
                Debug.Log("We are trying to see if the charge is over or not");
                return true;
            }
        }
        return false;
    }
}
