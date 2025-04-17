using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "FSM/AI/Actions/dying", fileName = "A_Dying")]

public class Dying_Action : Action
{
    public override void Act(Blackboard blackboard)
    {
        if (blackboard is AIBlackBoard aiBlackboard)
        {
            Debug.Log("Enemy is in the dying state");
            ReduceSpeed(aiBlackboard);
            DisableMonster(aiBlackboard);

            
        }
    }
    private void ReduceSpeed(AIBlackBoard aiBlackboard)
    {
        aiBlackboard.navMeshAgent.speed = 0;
        aiBlackboard.navMeshAgent.acceleration = 0;
        aiBlackboard.navMeshAgent.isStopped = true;
        aiBlackboard.navMeshAgent.GetComponent<NavMeshAgent>().enabled = false;

    }
    private void DisableMonster(AIBlackBoard aiBlackboard)
    {
        var gork =  aiBlackboard.navMeshAgent.GetComponent<Rigidbody>();
        gork.detectCollisions = false;
    }
}
