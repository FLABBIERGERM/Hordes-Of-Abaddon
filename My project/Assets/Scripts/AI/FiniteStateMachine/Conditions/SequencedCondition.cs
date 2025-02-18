using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Condition/SequencedCondition", fileName = "Sequenced_condition")]
public class SequencedCondition : Condition
{
    [SerializeField] private List<Condition> conditions = new List<Condition>();

    public override bool Evaluate(Blackboard blackboard)
    {
        foreach (var condition in conditions)
        {
            if (!condition.Evaluate(blackboard))
            {
                return false;
            }

        }
        return true;
    }
}
