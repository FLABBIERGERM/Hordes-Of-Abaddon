using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "AI/Actions/FormFullyChanged", fileName = "Angel_FormtwoChangeAction")]

public class Angel_ChangingForm : Action
{
    public override void Act(Blackboard blackboard)
    {
        if (blackboard is AIBlackBoard aiBlackboard)
        {
            aiBlackboard.navMeshAgent.isStopped = true;
        }
    }
}
