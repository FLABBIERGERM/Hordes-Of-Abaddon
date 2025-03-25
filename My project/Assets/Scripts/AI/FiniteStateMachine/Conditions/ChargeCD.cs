using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "FSM/AI/Conditions/Charge cooldown", fileName = "C_ChargeCoolDown")]

public class ChargeCD : Condition
{

    public override bool Evaluate(Blackboard blackboard)
    {
        if (blackboard is AIBlackBoard aIblackboard)// this is now working perfectly
        {
            if (aIblackboard.chargeOver && aIblackboard.IsChargeCDR())
            {
                aIblackboard.ResetCCD();
                aIblackboard.chargeOver = false;
                return true;
            }
        }
        return false;
    }
}
