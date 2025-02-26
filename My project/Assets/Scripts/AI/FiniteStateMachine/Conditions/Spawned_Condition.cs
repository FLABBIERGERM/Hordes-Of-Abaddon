using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "FSM/AI/Conditions/SpawnedCondition", fileName = "C_Spawned")]
public class Spawned_Condition : Condition
{
    private EnemyAnimationController enemyAnimationController;
    public override bool Evaluate(Blackboard blackboard)
    {
        if(blackboard is AIBlackBoard aiBlackBoard)
        {
            if( aiBlackBoard.spawned == true)
            {
                return true;
            }
        }
        return false;
    }

}
