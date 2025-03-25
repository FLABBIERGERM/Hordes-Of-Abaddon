using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;


[CreateAssetMenu(menuName = "FSM/AI/Actions/Charge", fileName = "A_Charge")]

public class ChargeAttack : Action
{
    public override void Act(Blackboard blackboard)
    {
        if (blackboard is AIBlackBoard aiBlackboard)
        {
            Chargeattack(aiBlackboard);
        }
    }
    private void Chargeattack(AIBlackBoard aiBlackBoard)
    {
        var originalSpeed = aiBlackBoard.navMeshAgent.speed;
        //if (Vector3.Distance(aiBlackBoard.navMeshAgent.transform.position, aiBlackBoard.chargeLocation) <= 2f)
        //{
        //    Debug.Log("So it is checking the distance from the charge location");
        //    aiBlackBoard.chargeOver = true;
        //    aiBlackBoard.navMeshAgent.speed = originalSpeed;
        //    Debug.Log("Original speed" + aiBlackBoard.navMeshAgent.speed);
        //}
        if (!aiBlackBoard.chargeOver)
        {
            aiBlackBoard.navMeshAgent.speed = originalSpeed * 3;
            aiBlackBoard.navMeshAgent.isStopped = false;
            aiBlackBoard.navMeshAgent.destination = aiBlackBoard.chargeLocation;
        }
    }
}
