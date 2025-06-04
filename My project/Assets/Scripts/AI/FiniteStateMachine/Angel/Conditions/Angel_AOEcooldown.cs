using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Angel_AOEcooldown : Condition
{
    public override bool Evaluate(Blackboard blackboard)
    {
        if(blackboard is AIBlackBoard aIblackboard)
        {

            if (aIblackboard.HeavensDescentCD())
            {
                return true;
            }
        }
        return false;
    }
}
