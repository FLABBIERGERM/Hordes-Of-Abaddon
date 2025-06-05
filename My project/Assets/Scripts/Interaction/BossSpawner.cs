using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour, iinteractible
{
    [SerializeField] private GameObject enemyBoss;
    [SerializeField] private GameObject spawnLocation;
    [SerializeField] public Transform playerToFollow;
    public void Interact(CharacterInteractManager characterInteractManager, CharacterMovement character)
    {
        AiStateController stateController = enemyBoss.GetComponent<AiStateController>();
        if (stateController != null && stateController.aiBlackboard != null)
        {
            stateController.aiBlackboard.chaseTarget = playerToFollow;
            stateController.aiBlackboard.chargeLocation = playerToFollow.position;
            // stateController.aiBlackboard.chargeOver = false;
        }
        if (HudScore.Instance.essence >= 5000)
        {
            HudScore.Instance.essence -= 5000;
            Instantiate(enemyBoss, spawnLocation.transform.position, Quaternion.identity);
        }
        Debug.Log("POOOOOOOOOOOOR");
    }

}
