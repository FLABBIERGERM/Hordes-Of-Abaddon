using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "AI/Actions/Angel Fire Beam")]
public class AngelBeamCcannonFire : Action
{
    public override void Act(Blackboard blackboard)
    {
        if (blackboard is AIBlackBoard aIblackboard)
        {
            if (aIblackboard.enemyAnimationController != null && aIblackboard.chaseTarget != null)
            {
                aIblackboard.enemyAnimationController.FireAngelBeamAtTarget();


                aIblackboard.ResetACD();
            }
        }
    }

}
