using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Conditions/AOE_Ended", fileName = "Angel_AoeEnded")]

public class Angel_AOEAttackOver : Condition
{
    public override bool Evaluate(Blackboard blackboard)
    {
        if (blackboard is AIBlackBoard aIblackboard)
        {
            if(aIblackboard.aoeOver == true)
            {
                return true;

            }
        }
        return false;
    }
}
