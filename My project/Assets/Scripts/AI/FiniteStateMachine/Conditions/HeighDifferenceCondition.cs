using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/AI/Conditions/HeightDifferenceCondition", fileName = "C_HDifference")]
public class HeighDifferenceCondition : Condition
{
    [SerializeField] private float heightDifference = 5f;
    private bool NavNegative = false;
    private bool ChaseNegative = false;
    public override bool Evaluate(Blackboard blackboard)
    {
        
        if (blackboard is AIBlackBoard aiBlackBoard && aiBlackBoard.chaseTarget != null)
        {
            CheckIfNegative(blackboard);
            Debug.Log("We are inside the height difference condition");
            if (NavNegative == true && ChaseNegative == true)
            {
                Debug.Log("Both the navagent and chase target are negative" + aiBlackBoard.navMeshAgent.transform.position.y + aiBlackBoard.chaseTarget.transform.position.y );
                return ((aiBlackBoard.navMeshAgent.transform.position.y *-1) + (aiBlackBoard.chaseTarget.transform.position.y *-1)) / 2 <= heightDifference;

            }
            if (NavNegative == true && ChaseNegative != true)
            {
                Debug.Log("Just the nave agent is negative the chase target is not" + aiBlackBoard.navMeshAgent.transform.position.y + aiBlackBoard.chaseTarget.transform.position.y);

                return ((aiBlackBoard.navMeshAgent.transform.position.y * -1) + aiBlackBoard.chaseTarget.transform.position.y ) / 2 <= heightDifference;

            }
            if (NavNegative != true && ChaseNegative == true)
            {
                Debug.Log("The nav agent is not negative but the chase target is" + aiBlackBoard.navMeshAgent.transform.position.y + aiBlackBoard.chaseTarget.transform.position.y);
                return (aiBlackBoard.navMeshAgent.transform.position.y + (aiBlackBoard.chaseTarget.transform.position.y * -1)) / 2 <= heightDifference;

            }
            if (NavNegative != true && ChaseNegative != true)
            {
                Debug.Log("Neither of them are negative" + aiBlackBoard.navMeshAgent.transform.position.y + aiBlackBoard.chaseTarget.transform.position.y);
                return (aiBlackBoard.navMeshAgent.transform.position.y + aiBlackBoard.chaseTarget.transform.position.y)/2 <= heightDifference;
            }
        }
        return false;
    }

    private void CheckIfNegative(Blackboard blackboard)
    {
       if(blackboard is AIBlackBoard aiBlackBoard)
        {
           if( aiBlackBoard.navMeshAgent.transform.position.y < 0)
            {
                NavNegative = true;
            }
            else
            {
                NavNegative = false;
            }
           if(aiBlackBoard.chaseTarget.transform.position.y < 0)
            {
                ChaseNegative = true;
            }
            else
            {
                ChaseNegative= false;
            }
        }
    }
}
