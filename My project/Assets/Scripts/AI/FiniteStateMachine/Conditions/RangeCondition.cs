using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/AI/Conditions/ChargeDistanceCondition", fileName = "C_CDistance")]
public class RangeCondition : Condition
{
    [SerializeField] private float ChargeRange = 25f;
    public override bool Evaluate(Blackboard blackboard)
    {
        if(blackboard is AIBlackBoard aiBlackBoard && aiBlackBoard.chaseTarget != null)
        {
            Debug.Log("We are checking the charge distance");
            return Vector3.Distance(aiBlackBoard.navMeshAgent.transform.position, aiBlackBoard.chaseTarget.position) <= ChargeRange;
        }
        Debug.Log("Charge distance is to long");
        return false;
    }
}


