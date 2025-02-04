using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Condition/NotACondition", fileName = "C_NotAC")]

public class NotACondition : Condition
{
    [SerializeField] private Condition condisation;

    public override bool Evaluate(Blackboard blackboard)
    {
        if (condisation == null)
        {
            Debug.Log("There isnt a condition woops");
            return false; //  guess i should technically return true if its supposed to be opposites haha
        }

        return !condisation.Evaluate(blackboard); // should return the opposite.
    }
}
