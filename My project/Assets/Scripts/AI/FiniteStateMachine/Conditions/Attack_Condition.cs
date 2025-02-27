using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "FSM/AI/Conditions/Attack_Condition", fileName = "C_attack")]
public class Attack_Condition : Condition
{
    public float attackCoolDown = 2.25f;
    public bool canHitAgain = true;

    public override bool Evaluate(Blackboard blackboard)
    {
        if (blackboard is AIBlackBoard aIblackboard)
        {
            bool canAttack = Vector3.Distance(aIblackboard.navMeshAgent.transform.position, aIblackboard.chaseTarget.position) <= aIblackboard.attackRange;
           
            if (canAttack && canHitAgain)
            {
                canHitAgain = false;
                return true;
            }
            while(canHitAgain == false)
            {
                attackCoolDown -= Time.deltaTime;
                if(attackCoolDown <= 0)
                {
                    canHitAgain = true;
                    attackCoolDown = 2.25f;
                }
                break;

            }
        }
        return false;
    }
}
