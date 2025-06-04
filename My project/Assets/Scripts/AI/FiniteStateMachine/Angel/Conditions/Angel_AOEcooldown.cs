using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Conditions/AOECD", fileName = "Angel_AoeCooldownCheck")]
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
