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
    Purchase,
    primaryUnlock
}
public class Interactibles : MonoBehaviour, iinteractible
{
    [SerializeField] private GameObject bossSpawn;
    [SerializeField] private Transform bossSpawnLocation;
    [SerializeField] private AudioSource myAudio = null;
    [SerializeField] private PlayerController playerController;
    public InteractionVariation InteractionVariation;
    [SerializeField] public GameObject rifle;

    [SerializeField] private string roomToUnlockString;
    [SerializeField] private Room_Manager.Room roomToUnlock;

    private bool used = false;
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

            case InteractionVariation.primaryUnlock:
                buyGunOne();
                break;
        }
    }

    private void SoundPlay()
    {
        myAudio.Play();
    }
    private void DoorOpen()
    {
        if (used) return;
        used = true;
        if(HudScore.Instance.essence < 1250)
        {
            return;
        }
        if(HudScore.Instance.essence >= 1250)
        {
            HudScore.Instance.essence -= 1250;
            Room_Manager.Instance.Unlockroom(roomToUnlockString);
            Destroy(gameObject);
        }
      
    }
    private IEnumerator DestroyNextFrame()
    {
        yield return null;
        Destroy(gameObject);
    }
    private void buyGunOne()
    {
        if (HudScore.Instance.essence >= 2500 && playerController.primary != rifle)
        {
            HudScore.Instance.essence -= 2500;
            playerController.primary = rifle;
        }
    }
    private void Repair()
    {
        // decide if i want this later, i think i do but for now its gonna be buy wall weapon

        if(HudScore.Instance.essence >= 2000)
        {
            HudScore.Instance.essence -= 2000;
            playerController.currentActive.GetComponent<SideArm>().totalAmmo += 40;
        }
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
