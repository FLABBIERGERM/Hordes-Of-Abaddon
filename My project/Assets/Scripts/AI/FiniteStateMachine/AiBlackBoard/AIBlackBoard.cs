using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class AIBlackBoard : Blackboard
{

    [Tooltip("The navmesh agent that the owning AIstatecontroller uses for movement and navigation")]
    public NavMeshAgent navMeshAgent;


    [Header("Chase AI")]
    [Tooltip("The transform of the object the AI is navigating towards")]
    public Transform chaseTarget;

    public float attackRange = 2.0f;

    [Header("perception")]
    [Tooltip("position from which ai performs visual perception like looking")]
    public Transform eyes;

    [Tooltip("The length of the spherecast this agent using in loking")]
    public float lookRange = 8f;

    [Tooltip("radius of the spherecast this agent uses in look conditions")]
    public float lookSphereCastRadius = 1f;
    public bool canSeePlayer = false;

    [Header("Searching")]
    public float searchDuration = 8f;

    [Tooltip("the speed at which this ai wil turn in scan states")]
    public float searchingTurnSpeed = 120f;


    [Tooltip("This will be the spawning information like where to spawn")]
    private List<GameObject> spawnSpots;

    // will have to change a bit of this as i dont actually need them to not know where the player is
    // otherwise its just chase, attack and then die i guess
    public AudioSource attackAudioSource;
}

