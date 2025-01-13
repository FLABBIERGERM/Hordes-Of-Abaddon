using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "FSM/State", fileName = "State")]
public class State : ScriptableObject
{
    [Tooltip("A list of all actions to execute when this state is entered.")]
    [SerializeField] private Action[] enterActions;
    [Tooltip("A list of all actions to execute when this state is called in the Update loop")]
    [SerializeField] private Action[] updateActions;
    [Tooltip("A list of all Actions to execute when this state is exited")]
    [SerializeField] private Action[] exitActions;

    [Tooltip("A list of all possible transitions out of this state")]
    [SerializeField] private Transition[] transitions;

    public void EnterState(Blackboard blackboard)
    {
        foreach (Action enterAction in enterActions)
        {
            enterAction.Act(blackboard);
        }
    }

    public void UpdateState(Blackboard blackboard)
    {
        foreach(Action updateAction in updateActions)
        {
            updateAction.Act(blackboard);
        }
    }

    public void ExitState(Blackboard blackboard)
    {
        foreach (Action exitAction in exitActions)
        {
            exitAction.Act(blackboard);
        }
    }
    private void CheckTransitions(Blackboard blackboard)
    {
        foreach (Transition transition in transitions)
        {
            bool conditionSucceeded = transition.condition.Evaluate(blackboard);

            if (conditionSucceeded)
            {
                blackboard.owningController.TransitionToState(transition.nextState);

                break;
            }
        }
        
    }
}
