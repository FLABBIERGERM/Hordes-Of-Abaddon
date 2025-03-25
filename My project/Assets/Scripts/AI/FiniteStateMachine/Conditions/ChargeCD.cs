using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "FSM/AI/Conditions/chargecd_condition", fileName = "CCD_condition")]

public class ChargeCD : Condition
{

    public override bool Evaluate(Blackboard blackboard)
    {
        if (blackboard is AIBlackBoard aIblackboard)// this is now working perfectly
        {
            if (aIblackboard.chargeOver && aIblackboard.IsChargeCDR())
            {
                aIblackboard.ResetCCD();
                return true;
            }
        }
        return false;
    }
}
