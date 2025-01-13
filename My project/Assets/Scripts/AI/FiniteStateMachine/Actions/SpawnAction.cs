using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "FSM/AI/Actions/Spawn", fileName = "A_Spawn")]

public class SpawnAction : Action
{
    public override void Act(Blackboard blackboard)
    {
        if (blackboard.owningController is not AiStateController controller) return;

    }
}
