using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/AI/Actions/Jump_Attack", fileName = "A_JAttack")]

public class Jump_Attack : Action
{

    public override void Act(Blackboard blackboard)
    {
        if (blackboard is AIBlackBoard aiBlackboard)
        {
            PerformJAttack(aiBlackboard);
        }
    }
    private void PerformJAttack(AIBlackBoard aiBlackboard)
    {
        float PDistance = Vector3.Distance(aiBlackboard.navMeshAgent.transform.position, aiBlackboard.chaseTarget.position);
        Debug.Log("The distance Between the Enemy Jumping and The Player is: " +  PDistance);
        if(PDistance >= 20)
        {
            // This is where the lerp will go i think. and i also think we dont need the if statment as that would be a conditional thing i think.
        }

    }
}
