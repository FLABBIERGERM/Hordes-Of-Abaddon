using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
[CreateAssetMenu(menuName = "FSM/AI/Actions/ChargeOver", fileName = "A_ChargeOver")]

public class ChargeOverAction : Action
{
    public override void Act(Blackboard blackboard)
    {
        if (blackboard is AIBlackBoard aiBlackboard)
        {
            ChargeEnd(aiBlackboard);
        }
    }
    private void ChargeEnd(AIBlackBoard aiBlackBoard)
    {
        var originalSpeed = aiBlackBoard.navMeshAgent.speed / 3;

        if (Vector3.Distance(aiBlackBoard.navMeshAgent.transform.position, aiBlackBoard.chargeLocation) < 1f)
        {
            Debug.Log("So it is checking the distance from the charge location");
            aiBlackBoard.owningController.GetComponent<Animator>()?.SetTrigger("SlamAttack");
            aiBlackBoard.navMeshAgent.GetComponent<AudioSource>().Stop();
           // Instantiate(aiBlackBoard.chargeCrash,aiBlackBoard.chargeLocation, Quaternion.identity);
            aiBlackBoard.chargeOver = true;
            aiBlackBoard.owningController.GetComponent<Animator>()?.SetBool("Stunned", true);

            aiBlackBoard.navMeshAgent.speed = originalSpeed;
            aiBlackBoard.ChargingAfk();
            //aiBlackBoard.navMeshAgent.GetComponent<NavMeshAgent>().enabled = false;
            Debug.Log("Original speed" + aiBlackBoard.navMeshAgent.speed);
        }
    }
}
