using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public enum InteractionVariation
{
    Playsound,
    Destroy,
    Fix,
    Purchase
}
public class Interactibles : MonoBehaviour, iinteractible
{
    [SerializeField] private GameObject bossSpawn;
    [SerializeField] private Transform bossSpawnLocation;
    [SerializeField] private AudioSource myAudio = null;
    public InteractionVariation InteractionVariation;



    public void Interact(CharacterInteractManager characterInteractManager, CharacterMovement character)
    {
        switch (InteractionVariation)
        {
            case InteractionVariation.Playsound:
                SoundPlay();
                break;
            case InteractionVariation.Destroy:
                DoorOpen();
                break;
            case InteractionVariation.Fix:
                Repair();
                break;
            case InteractionVariation.Purchase:
                Buy();
                break;
            
        }
    }

    private void SoundPlay()
    {
        myAudio.Play();
    }
    private void DoorOpen()
    {
        Destroy(gameObject);
    }
    private void Repair()
    {
        // decide if i want this later
    }
    private void Buy() // needs code for this
    {
        Debug.Log("You are trying to buy the boss");
        if(HudScore.Instance.essence >= 5000f)
        {
            HudScore.Instance.essence -= 5000f;
            Instantiate(bossSpawn, bossSpawnLocation.position, Quaternion.identity);
        }
        Debug.Log("But you are way to poor");
    }
}
