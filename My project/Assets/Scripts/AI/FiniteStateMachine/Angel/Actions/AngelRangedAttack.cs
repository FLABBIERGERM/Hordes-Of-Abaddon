using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Angel_RangedAttackPhase1", fileName = "AP1_Ranged")]
public class AngelRangedAttack : Action
{
    public override void Act(Blackboard blackboard)
    {
        if (blackboard is AIBlackBoard aiBlackboard)
        {
            PerformAttack(aiBlackboard);
        }
    }

    private void PerformAttack(AIBlackBoard aiBlackboard)
    {
        var animator = aiBlackboard.enemyAnimationController?.Aanimator;

        if (animator == null) { return; }

        animator.SetBool("Base_Melee", false);
        animator.SetBool("Base_Range", true);
        animator.SetTrigger("Punched");

        if(aiBlackboard.angelBeamCannons.Count > 1)
        {
            aiBlackboard.angelBeamCannons[0]?.FireAt(aiBlackboard.chaseTarget);
            aiBlackboard.angelBeamCannons[8]?.FireAt(aiBlackboard.chaseTarget);

        }
        aiBlackboard.ResetShootCD();

    }

}
