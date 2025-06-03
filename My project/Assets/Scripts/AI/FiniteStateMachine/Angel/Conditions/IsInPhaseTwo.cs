using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Conditions/IsInPhaseTwo")]
public class IsInPhaseTwo : Condition
{
    public override bool Evaluate(Blackboard blackboard)
    {
        if (blackboard is AIBlackBoard aIblackboard)
        {
            return aIblackboard.currentPhase == 2f;
        }
        return false;
    }
}
