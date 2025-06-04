using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Conditions/AngelChangedtoForm2", fileName = "Angel_FormtwoChange")]

public class Form_fullychanged : Condition
{
    public override bool Evaluate(Blackboard blackboard)
    {
        if (blackboard is AIBlackBoard aIblackboard)
        {
            if (aIblackboard.owningController.GetComponent<Animator>()?.GetBool("Base_Range") != true)
            {
                aIblackboard.owningController.GetComponent<Animator>()?.SetBool("Base_Range", true);

            }

            return aIblackboard.formChanged;
        }
        return false;
    }
}
