using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName ="AI/Actions/MaintainDistanceAndRoam")]
public class MaintainDistanceAndRoam : Action
{
    private float nextRoamTime = 0;

    public override void Act(Blackboard blackboard)
    {
        if (blackboard is AIBlackBoard aiBlackboard)
        {
            float distance = Vector3.Distance(aiBlackboard.navMeshAgent.transform.position, aiBlackboard.chaseTarget.position);
            Vector3 toPlayer = (aiBlackboard.navMeshAgent.transform.position- aiBlackboard.chaseTarget.position).normalized;
           

            Vector3 desiredPosition;

            if (distance < aiBlackboard.distanceFromPlayer - aiBlackboard.errorRange)
            {
                desiredPosition = aiBlackboard.navMeshAgent.transform.position - toPlayer * 5f;
            }
            else if(distance > aiBlackboard.distanceFromPlayer + aiBlackboard.errorRange) 
            {
                desiredPosition = aiBlackboard.navMeshAgent.transform.position + toPlayer * 5f;
            }
            else if(Time.time >= nextRoamTime)
            {
                Vector3 lateral = Vector3.Cross(Vector3.up, toPlayer);
                float dir = Random.value > 0.5f ? 1f : -1f;
                desiredPosition = aiBlackboard.navMeshAgent.transform.position + lateral * dir * Random.Range(3f, 6f);
                nextRoamTime = Time.time + Random.Range(2f,4f);
            }
            else
            {
                return;
            }

            if(NavMesh.SamplePosition(desiredPosition,out NavMeshHit hit, 3f, NavMesh.AllAreas))
                {
                aiBlackboard.navMeshAgent.SetDestination(hit.position);
            }
        }

    }
}
