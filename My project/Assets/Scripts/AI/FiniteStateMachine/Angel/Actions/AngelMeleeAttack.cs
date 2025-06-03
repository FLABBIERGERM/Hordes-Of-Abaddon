using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Angel_MeleeAttack", fileName = "AP1_Melee")]
public class AngelMeleeAttack : Action
{
    public override void Act(Blackboard blackboard)
    {
        if (blackboard is AIBlackBoard aiBlackboard)
        {
            PerformAttack(aiBlackboard);

        }
    }// i shouldnt need to worry about distance as the condition to get in here will be if its within 3M then its Melee and if its over that itl be a ranged shot
    private void PerformAttack(AIBlackBoard aiBlackboard)
    {
        var animator = aiBlackboard.enemyAnimationController?.Aanimator;

        if (animator == null) { return; }

        animator.SetBool("Base_Melee", true);
        animator.SetBool("Base_Range", false);
        animator.SetTrigger("Punched");
        animator.SetFloat("LeftorRight", Random.Range(1, 2));
        // meele damage logic goes here
        GameManager.Instance.TookDamage(-aiBlackboard.enemyDamage);
    }
}