using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;


[CreateAssetMenu(menuName = "FSM/AI/Actions/Chase", fileName = "A_chase")]
public class ChaseAction : Action
{
    // [SerializeField] private AudioSource ChasingNoise = null;
     //private SpawnManager spawnManager = new SpawnManager();

    public override void Act(Blackboard blackboard)
    {

        if (blackboard.owningController is not AiStateController controller) return;

        if(blackboard is AIBlackBoard aIBlackBoard && aIBlackBoard.chaseTarget != null)
        {
            aIBlackBoard.owningController.GetComponent<Animator>()?.SetBool("Attacking", false);

            aIBlackBoard.navMeshAgent.destination = aIBlackBoard.chaseTarget.position;
            aIBlackBoard.navMeshAgent.isStopped = false;

        }
    }
}
