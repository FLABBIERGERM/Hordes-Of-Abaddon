using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class CharacterInteractManager : MonoBehaviour
{

    List<iinteractible> interactableObjects = new List<iinteractible>();

    public CharacterMovement owningCharacter;

    public UnityEvent OnInteractablesExist;

    public UnityEvent OnInterctablesDoNotExist;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<iinteractible>(out iinteractible interactable))
        {
            TrackInteractable(interactable);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<iinteractible>(out iinteractible interactable))
        {
            StopTrackingInteractable(interactable);
        }
    }


    public void Interact()
    {
        interactableObjects.RemoveAll(x => x == null);

        if (interactableObjects.Count <= 0) { return; }
        
        iinteractible target = interactableObjects[0];

        interactableObjects.RemoveAt(0);

        if (target != null)
        {
            target.Interact(this, owningCharacter);
        }
    }

    public void TrackInteractable(iinteractible interactableToTrack)
    {
        interactableObjects.Add(interactableToTrack);

        if(interactableObjects.Count == 1)
        {
            OnInteractablesExist.Invoke();
        }
    }

    public void StopTrackingInteractable(iinteractible trackedInteractable)
    {
        if (interactableObjects.Contains(trackedInteractable))
        {
            interactableObjects.Remove(trackedInteractable);
            if(interactableObjects.Count == 0)
            {
                OnInterctablesDoNotExist.Invoke();
            }
        }
    }
}
