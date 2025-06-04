using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
[CreateAssetMenu(menuName = "AI/Actions/AngelPHase2AOE_Buildup", fileName = "AOE_Buildup")]

public class AOE_Buildup : Action
{
    public override void Act(Blackboard blackboard)
    {
        if (blackboard is AIBlackBoard aiBlackboard)
        {
            Babylon(aiBlackboard);
        }
    }

    private void Babylon(AIBlackBoard aiBlackBoard)
    {
        if(aiBlackBoard.aoeCharging == false )
        {
            aiBlackBoard.aoeCharging = true;
            aiBlackBoard.navMeshAgent.isStopped = true;
            // maybe later on adding in audio calls here
        }
    }
}
