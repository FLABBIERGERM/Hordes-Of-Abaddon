using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Conditions/IsInMeleeRange", fileName = "AC_MeleeRange")]
public class Angel_Melee_Condition : Condition
{
    public override bool Evaluate(Blackboard blackboard)
    {
        if (blackboard is AIBlackBoard aIblackboard)
        {
            bool canAttack = Vector3.Distance(aIblackboard.navMeshAgent.transform.position, aIblackboard.chaseTarget.position) <= aIblackboard.aMeleeDistance;

            if (canAttack && aIblackboard.IsattackCDR())
            {
                // aIblackboard.ResetACD();
                return true;
            }
        }
        return false;
    }
}
