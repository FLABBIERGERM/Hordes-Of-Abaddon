using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class AIBlackBoard : Blackboard
{

    [Tooltip("The attack cooldown section")]
    public float attackCoolDown = 2.25f;
    public float nextAttack = 0.0f;

    public float chargeCoolDown = 45f;
    public float nextCharge = 0.0f;

    public float chargeAfk = 2.0f;
    public float chargeAfkOver = 0.0f;

    public Vector3 chargeLocation;

    public bool chargeOver = false;

    public Object chargeCrash;                   
    public bool IsattackCDR()
    {
        return Time.time >= nextAttack;
    }
    public void ResetACD()
    {
        nextAttack = Time.time + attackCoolDown;
    }

    public bool IsChargeCDR()
    {
        Debug.Log("This is the charge cool down being over or not" + (Time.time >= nextCharge));
        return Time.time >= nextCharge;
    }
    public void ResetCCD()
    {
        nextCharge = Time.time + chargeCoolDown;
        Debug.Log("Reseting time actual time" + nextCharge);
    }
    public bool AfterChargeAFK()
    {
     // Debug.Log("Checking the times for the charge to see if its good, heres Time.time: " + Time.time + "Heres the chargeAfk" + chargeAfk);
        return Time.time >= chargeAfkOver;
    }
    public void ChargingAfk()
    {
        chargeAfkOver = Time.time + chargeAfk;
    }

    [Tooltip("The navmesh agent that the owning AIstatecontroller uses for movement and navigation")]
    public NavMeshAgent navMeshAgent;
    
    public Rigidbody rigidBody;

    [Header("Chase AI")]
    [Tooltip("The transform of the object the AI is navigating towards")]
    public Transform chaseTarget ; // gotta figure this bit out  for new instanced enemys. losing my got damn mind about it now to.

    public float attackRange = 3.0f;

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


    [Tooltip("This will be the enemy damage per enemy")]
    public int enemyDamage; // i want to change all of these to floats for health idk why but i do.

    [Tooltip("This will be the spawning information like where to spawn")]
    private List<GameObject> spawnSpots;

    // will have to change a bit of this as i dont actually need them to not know where the player is
    // otherwise its just chase, attack and then die i guess
    public AudioSource attackAudioSource;

    public bool spawned = false;


}

