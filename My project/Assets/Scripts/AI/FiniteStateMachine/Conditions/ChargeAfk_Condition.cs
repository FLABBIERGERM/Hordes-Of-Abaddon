using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/AI/Conditions/ChargeAFK_Condition", fileName = "C_CAFK")]

public class ChargeAfk_Condition : Condition
{
    public override bool Evaluate(Blackboard blackboard)
    {

        if (blackboard is AIBlackBoard aiBlackBoard)
        {
            if (aiBlackBoard.chargeOver && aiBlackBoard.AfterChargeAFK()){
                aiBlackBoard.ChargingAfk();
                aiBlackBoard.navMeshAgent.isStopped = false;
                aiBlackBoard.owningController.GetComponent<Animator>()?.SetBool("Stunned", false);

                return true;
            }
        }
        return false;
    }
}
