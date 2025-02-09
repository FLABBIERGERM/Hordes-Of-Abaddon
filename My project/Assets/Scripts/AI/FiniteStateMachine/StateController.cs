using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    //StateControl
    [Tooltip("The context that our controller is currently in")]
    public State currentState;

    //blackboard variables
    [Tooltip("saved info field for our context typically called blackbaord")]
    public Blackboard blackboard;

    private void Awake()
    {
        InitializeStateController();
    }

    private void Update()
    {
        blackboard.IncrementStateTime(Time.deltaTime);

        currentState.UpdateState(blackboard);

    }

    public virtual void InitializeStateController()
    {
        // perform any initialization here
    }

    public bool TransitionToState(State nextState)
    {
        if(nextState != null)
        {   
            currentState.ExitState(blackboard);

            blackboard.ResetStateTime();

            Debug.Log("Transition to state:" + nextState);
            currentState = nextState;
            currentState.EnterState(blackboard);
            return true;
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        if(currentState != null)
        {
            GUIStyle handlesStyle = new GUIStyle();
            handlesStyle.normal.textColor = Color.red;
            handlesStyle.fontSize = 24;
            UnityEditor.Handles.Label(transform.position, currentState.name, handlesStyle);
        }
    }
}
