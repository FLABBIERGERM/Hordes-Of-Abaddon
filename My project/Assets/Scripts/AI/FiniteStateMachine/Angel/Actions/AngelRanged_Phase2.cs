using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Angel_RangedAttack", fileName = "AP2_RangedAttack")]

public class AngelRanged_Phase2 : Action
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

        animator.SetBool("Base_Range", true);
        animator.SetTrigger("Punched");

        if (aiBlackboard.angelBeamCannons.Count > 0)
        {
            int HandTofire = Random.Range(0, aiBlackboard.angelBeamCannons.Count);
            // this animation section is used to determine which of the hands is 1 firing the beam and 2 animating. Look in the animation controller for more
            animator.SetInteger("HandIndex", HandTofire);
            aiBlackboard.angelBeamCannons[HandTofire]?.FireAt(aiBlackboard.chaseTarget);
        }
        aiBlackboard.ResetACD();
    }
}
