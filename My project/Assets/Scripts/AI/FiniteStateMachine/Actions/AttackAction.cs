using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;


[CreateAssetMenu(menuName = "FSM/AI/Actions/Attack", fileName = "A_Attack")]
public class AttackAction : Action
{
    public override void Act(Blackboard blackboard)
    {
        if(blackboard is AIBlackBoard aiBlackboard)
        {

            PlayAttackAnimation(aiBlackboard);
            PerformAttack(aiBlackboard);
        }
    }

    private void PlayAttackAnimation(AIBlackBoard aiBlackboard)
    {
        aiBlackboard.owningController.GetComponent<Animator>()?.SetTrigger("Punched");
    }
    private void PerformAttack(AIBlackBoard aiBlackboard)
    {
        aiBlackboard.ResetACD();


        AudioSource audioSource = aiBlackboard.owningController.GetComponent<AudioSource>();
        aiBlackboard.owningController.GetComponent<Animator>()?.SetBool("Attacking", true);

        GameManager.Instance.TookDamage(-aiBlackboard.enemyDamage);
        if (audioSource != null && audioSource.clip != null)
        {
            aiBlackboard.attackAudioSource.Play();
        }
        if (aiBlackboard.owningController.GetComponent<Animator>().GetBool("Attacking"))
        {
            aiBlackboard.navMeshAgent.isStopped = true;
            aiBlackboard.navMeshAgent.speed = 0;
            aiBlackboard.navMeshAgent.acceleration = 0;
        }
    
        //else
        //{
        //    Debug.LogWarning("Audiosource or Audio Clip is missing on the AI GameObject");
        //}
    }
}
