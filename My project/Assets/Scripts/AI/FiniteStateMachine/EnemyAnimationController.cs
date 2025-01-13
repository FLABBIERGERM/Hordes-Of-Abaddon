using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimationController : MonoBehaviour
{

    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource footstepSoundclip;
    [SerializeField] private AudioClip stepin;

    private void Update()
    {
        animator.SetFloat("HorizontalSpeed", GetHorizontalAgentVelocity().magnitude);
        animator.SetBool("IsTraversingLink", navMeshAgent.isOnOffMeshLink);

    }

    private Vector3 GetHorizontalAgentVelocity()
    {
        return new Vector3(navMeshAgent.velocity.x, 0f, navMeshAgent.velocity.z);
    }
    public void EnemyWalk()
    {
        footstepSoundclip.PlayOneShot(stepin);
    }
}
