using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/AI/COnditions/DistanceConditions", fileName = "C_Distance")]
public class DistanceCondition : Condition
{
    [SerializeField] private float attackRange = 4.0f;

    public override bool Evaluate(Blackboard blackboard)
    {
        if(blackboard is AIBlackBoard aiBlackboard && aiBlackboard.chaseTarget != null)
        {
            return Vector3.Distance(aiBlackboard.navMeshAgent.transform.position, aiBlackboard.chaseTarget.position) <= attackRange;
        }
        return false; 
    }
}
