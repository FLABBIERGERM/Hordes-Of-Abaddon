using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "FSM/AI/Conditions/chargecd_condition", fileName = "CCD_condition")]

public class ChargeCD : Condition
{
    public float chargeCd = 45f;

    public bool canCharge = true;
    public override bool Evaluate(Blackboard blackboard)
    {
        if (blackboard is AIBlackBoard aIblackboard)
        {
            if(canCharge)
            {
                canCharge = false;
                return true;
            }
            while(canCharge == false)
            {
                chargeCd -= Time.deltaTime;
                if(chargeCd <= 0)
                {
                    canCharge = true;
                    chargeCd = 45f;
                }
                break;
            }
        }
        return false;
    }
}
