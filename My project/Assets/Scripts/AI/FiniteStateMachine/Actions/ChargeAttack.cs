using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;


[CreateAssetMenu(menuName = "FSM/AI/Actions/Charge", fileName = "A_Charge")]

public class ChargeAttack : Action
{
    private float chargeCd = 45;
    public override void Act(Blackboard blackboard)
    {

        if (blackboard is AIBlackBoard aiBlackboard)
        {
            ChargeA(aiBlackboard);
        }
    }

    private void ChargeA(AIBlackBoard aiBlackboard)
    {
        var originalSpeed = aiBlackboard.navMeshAgent.speed;

        aiBlackboard.navMeshAgent.speed = aiBlackboard.navMeshAgent.speed * 3;
        aiBlackboard.navMeshAgent.isStopped = false;

   
        while (aiBlackboard.navMeshAgent.transform.position != aiBlackboard.chargeLocation)
        {
            Debug.Log("Well its inside the while statment so it should be moving but i guess not");
            aiBlackboard.navMeshAgent.destination = aiBlackboard.chargeLocation;
            break;  
        }
        if (aiBlackboard.navMeshAgent.transform.position == aiBlackboard.chargeLocation) {
            aiBlackboard.chargeCd = chargeCd;
            aiBlackboard.chargeOver = true;
            aiBlackboard.navMeshAgent.speed = originalSpeed;
        }
    }

}
