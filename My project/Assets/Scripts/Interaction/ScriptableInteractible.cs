using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableInteractible : MonoBehaviour, iinteractible
{
    [SerializeField] private InteractBehaviorSO interactBehavior;
    public void Interact(CharacterInteractManager characterInteractManager, CharacterMovement character)
    {
        interactBehavior.Interact(characterInteractManager, character);

    }

}
