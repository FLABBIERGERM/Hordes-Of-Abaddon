using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractBehaviorSO : ScriptableObject
{
    public abstract void Interact(CharacterInteractManager characterInteractManager, CharacterMovement character);
}
