using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Conditions/ShotWaitOver", fileName = "AngelShotWait_Over")]

public class AngelShot_CD : Condition
{
    public override bool Evaluate(Blackboard blackboard)
    {
        if (blackboard is AIBlackBoard aIblackboard)
        {

            if (aIblackboard.angelShootCD())
            {
                return true;
            }
        }
        return false;
    }
}
