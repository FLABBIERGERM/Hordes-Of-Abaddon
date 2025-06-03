using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Conditions/IsPlayerInMeleeRange", fileName = "AP1_RangeCondition")]
public class POne_Range : Condition
{
    public override bool Evaluate(Blackboard blackboard)
    {
        if(blackboard is AIBlackBoard aiBlackboard)
        {
            float distance = Vector3.Distance(aiBlackboard.navMeshAgent.transform.position, aiBlackboard.chaseTarget.transform.position);
            return distance <= aiBlackboard.aMeleeDistance;
        }
        // if its false then its ranged attack if its true then its melee.
        return false;
    }
}
