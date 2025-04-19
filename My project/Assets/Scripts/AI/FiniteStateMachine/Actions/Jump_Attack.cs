using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/AI/Actions/Jump_Attack", fileName = "A_JAttack")]

public class Jump_Attack : Action
{
    private Vector3 speed;
    private Vector3 targetPos;

    public override void Act(Blackboard blackboard)
    {
        
        if (blackboard is AIBlackBoard aiBlackboard)
        {
            targetPos = aiBlackboard.chaseTarget.transform.position;
            GetJAttack(aiBlackboard);
        }
    }
    private void GetJAttack(AIBlackBoard aiBlackboard)
    {
        float PDistance = Vector3.Distance(aiBlackboard.navMeshAgent.transform.position, aiBlackboard.chaseTarget.position);
        Debug.Log("The distance Between the Enemy Jumping and The Player is: " +  PDistance);
        //if(PDistance >= 20)
        
            // This is where the lerp will go i think. and i also think we dont need the if statment as that would be a conditional thing i think.

            var dir = aiBlackboard.chaseTarget.transform.position - aiBlackboard.rigidBody.transform.position;
            var Height = dir.y;
            dir.y = 0;
            var distance = dir.magnitude;
            dir.y = distance;
            distance += Height;
            var vel = Mathf.Sqrt(distance * Physics.gravity.magnitude);
            speed = vel * dir.normalized;
            aiBlackboard.rigidBody.velocity = speed;
    }

}
