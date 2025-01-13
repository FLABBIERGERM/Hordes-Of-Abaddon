using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "FSM/COndition/StateTime", fileName = ("C_StateTime"))]
public class StateTimeCondition : Condition
{

    [Tooltip("The amount of time at which this condition evaluates to true")]
    [SerializeField] private float waitfortime = 3f;

    public override bool Evaluate(Blackboard blackboard)
    {
        return blackboard.CheckIfStateTimeElapsed(waitfortime);
    }

}
