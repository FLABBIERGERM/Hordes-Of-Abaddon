using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  abstract class Condition : ScriptableObject
{
    public abstract bool Evaluate(Blackboard blackboard);

}
