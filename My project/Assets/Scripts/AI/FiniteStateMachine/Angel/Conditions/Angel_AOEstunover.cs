using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Conditions/AngelStunOver", fileName = "Angel_STunOver")]
public class Angel_AOEstunover : Condition
{
    public override bool Evaluate(Blackboard blackboard)
    {
        if (blackboard is AIBlackBoard aIblackboard)
        {
            if (aIblackboard.afterAoeAFK())
            {
                aIblackboard.navMeshAgent.isStopped = false;
                return true;
            }
        }
        return false;
    }
}