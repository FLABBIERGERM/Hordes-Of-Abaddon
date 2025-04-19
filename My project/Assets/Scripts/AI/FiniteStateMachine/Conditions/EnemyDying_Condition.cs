using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/AI/Conditions/dying_Condition", fileName = "C_Dying")]

public class EnemyDying_Condition : Condition
{
    public override bool Evaluate(Blackboard blackboard)
    {
        if (blackboard is AIBlackBoard aIblackboard)
        {
          
            if(aIblackboard.dead == true)
            {
                return true;
            }
        }
        return false;
    }
}
