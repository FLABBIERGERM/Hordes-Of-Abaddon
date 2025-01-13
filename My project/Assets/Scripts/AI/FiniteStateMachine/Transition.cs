using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Transition 
{

    [Tooltip(" the condition being evaluated by this transition")]
    public Condition condition;


    [Tooltip(" the state to transition to if condition is true")]
    public State nextState;
}
