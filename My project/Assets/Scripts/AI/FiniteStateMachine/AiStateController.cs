using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]


public class AiStateController : StateController
{

    // ai variables

    [Tooltip(" saved info fields for context typically a black board")]

    public AIBlackBoard aiBlackboard;

    public override void InitializeStateController()
    {
        blackboard = aiBlackboard;
    }
}
