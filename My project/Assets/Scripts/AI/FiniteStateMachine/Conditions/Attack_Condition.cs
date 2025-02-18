using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "FSM/AI/Conditions/Attack_Condition", fileName = "C_attack")]
public class Attack_Condition : Condition
{
    [SerializeField] private float attackCoolDown = 1.25f;
    private float lastAttackTime;

    public override bool Evaluate(Blackboard blackboard)
    {
        if (blackboard is AIBlackBoard aIblackboard)
        {
            bool canAttack = Vector3.Distance(aIblackboard.navMeshAgent.transform.position, aIblackboard.chaseTarget.position) <= aIblackboard.attackRange;
            if (canAttack && Time.time >= lastAttackTime + attackCoolDown)
            {
                lastAttackTime = Time.time;
                return true;
            }
        }
        return false;
    }
}
