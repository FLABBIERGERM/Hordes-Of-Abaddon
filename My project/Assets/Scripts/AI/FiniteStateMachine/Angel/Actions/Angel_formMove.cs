using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "AI/Actions/FormFullyChangedExit", fileName = "Angel_FormtwoChangeActionExit")]

public class Angel_formMove : Action
{
    public override void Act(Blackboard blackboard)
    {
        if (blackboard is AIBlackBoard aiBlackboard)
        {
            aiBlackboard.navMeshAgent.isStopped = false;
            //aiBlackboard.navMeshAgent.updateRotation = false;
        }
    }
}
