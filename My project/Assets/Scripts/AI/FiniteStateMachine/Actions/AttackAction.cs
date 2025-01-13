using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


[CreateAssetMenu(menuName = "FSM/AI/Actions/Attack", fileName = "A_Attack")]
public class AttackAction : Action
{

    [SerializeField] private int damageAmount = 10;



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
        AudioSource audioSource = aiBlackboard.owningController.GetComponent<AudioSource>();

        if(audioSource != null && audioSource.clip != null)
        {
            aiBlackboard.attackAudioSource.Play();
            GameManager.Instance.TookDamage(5);
        }
        else
        {
            Debug.LogWarning("Audiosource or Audio Clip is missing on the AI GameObject");
        }
    }
}
