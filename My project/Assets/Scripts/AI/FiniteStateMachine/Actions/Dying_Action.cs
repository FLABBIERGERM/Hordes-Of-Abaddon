using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/AI/Actions/dying", fileName = "A_Dying")]

public class Dying_Action : Action
{
    public override void Act(Blackboard blackboard)
    {
        if (blackboard is AIBlackBoard aiBlackboard)
        {
            Debug.Log("Why is this getting called?");
            DisableMonster(aiBlackboard);

            ReduceSpeed(aiBlackboard);
        }
    }
    private void ReduceSpeed(AIBlackBoard aiBlackboard)
    {
        aiBlackboard.navMeshAgent.speed = 0;
        aiBlackboard.navMeshAgent.acceleration = 0;
        aiBlackboard.navMeshAgent.isStopped = true;
    }
    private void DisableMonster(AIBlackBoard aiBlackboard)
    {
        var gork =  aiBlackboard.navMeshAgent.GetComponent<Rigidbody>();
        gork.detectCollisions = false;
    }
}
