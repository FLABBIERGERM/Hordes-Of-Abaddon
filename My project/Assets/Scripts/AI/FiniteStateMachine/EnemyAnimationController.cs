using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimationController : MonoBehaviour
{

    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource footstepSoundclip;
    [SerializeField] private AudioClip stepin;
    [SerializeField] private AiStateController aiStateController;

    [SerializeField] private AudioClip mutantSpawn;
    [SerializeField] private AudioClip gruntSpawn;

    public bool spawned = false;

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
    public void Dying()
    {
        aiStateController.aiBlackboard.dead = true;
    }

    public void Slammed()
    {
        Instantiate(aiStateController.aiBlackboard.chargeCrash, aiStateController.aiBlackboard.chargeLocation, Quaternion.identity);
    }
    public void SpawnEnd()
    {
        animator.SetBool("Spawned", true);
        aiStateController.aiBlackboard.spawned = true;
    }

    public void SpawnStart()
    {
        if (mutantSpawn != null)
        {
            footstepSoundclip.PlayOneShot(mutantSpawn);
        }
        else
        {
            footstepSoundclip.PlayOneShot(gruntSpawn);
        }
    }
}
