using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Blackboard 
{
    // state controller that owns this blackboard
    public StateController owningController;


    // timer value that indicates how long the FSM has been in current state

    public float stateTimeElapsed;

    public void IncrementStateTime(float passedTime)
    {
        stateTimeElapsed += passedTime;
    }

    public void ResetStateTime() { 
        stateTimeElapsed = 0;
    }

    public bool CheckIfStateTimeElapsed(float duration)
    {
        return (stateTimeElapsed >= duration);
    }
}
