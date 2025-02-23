using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public abstract class Action : ScriptableObject
{
    public abstract void Act(Blackboard blackboard);

}
