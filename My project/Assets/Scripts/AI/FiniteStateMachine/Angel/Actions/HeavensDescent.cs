using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "AI/Actions/AngelPHase2AOE", fileName = "HeavensWrath")]

public class HeavensDescent : Action
{
    public override void Act(Blackboard blackboard)
    {
        if (blackboard is AIBlackBoard aiBlackboard)
        {
            HeraldsHorn(aiBlackboard);
            aiBlackboard.ResetHeavensDescentCD(); // this should reset the cd to the new needed number as this will be the exit action of actually going boom
        }
    }

    private void HeraldsHorn(AIBlackBoard aiBlackboard)
    {
        // this is where the code will go to fully blow up the build up is in the action called AOE_buildup


    }
}
