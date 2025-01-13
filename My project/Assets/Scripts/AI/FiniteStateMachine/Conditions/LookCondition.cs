using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/AI/LookCondition", fileName = "C_Look")]
public class LookCondition : Condition
{
    [SerializeField] private LayerMask visibilityLayers;


    public override bool Evaluate(Blackboard blackboard)
    {
        if (blackboard is AIBlackBoard aiBlackboard && aiBlackboard.chaseTarget != null)
        {
            RaycastHit hit;
            Vector3 direction = aiBlackboard.chaseTarget.position = aiBlackboard.eyes.position;

            if (Physics.Raycast(aiBlackboard.eyes.position, direction, out hit , aiBlackboard.lookRange, visibilityLayers))
            {
                if(hit.transform == aiBlackboard.chaseTarget)
                {
                    aiBlackboard.canSeePlayer = true;
                    return true;
                }
            }
            aiBlackboard.canSeePlayer = false; // change to true later
        }
        return false;
    }
}
// have to change this to basically make it always see the player but i could just increase range and remove the visibility layers so it sees eitehr way.