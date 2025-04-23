using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance { get; private set; }
    public enum RoundState { RoundBegin, RoundPlaying, RoundEnd }
    public UnityEvent roundIncrease;// gonna have this send out a ping letting a hud for rounds know what is going on with it. 
    // speaking of which you as in me will have to compile all of those huds sooner or later as theres no reason to have 1 for score, ammo, and rounds
    public RoundState currentRoundState;

    [SerializeField] AudioClip NewRound;
    [SerializeField] AudioSource RoundAudio;
    public int enemySpawned = 0;
    public int enemyAlive = 0;
    public int currentRound = 1;
    public int totalEnemies = 0;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Start()
    {
        SpawnManager.Instance.enemySpawned.AddListener(OnenemySpawnedReceived);
        if(currentRound != 1)
        {
            currentRound = 1;
        }
        RoundStart();
    }
    public void RegisterEnemy(BaseStats enemyStats)
    {
        enemyStats.enemyKilled.AddListener(RecivedOnEnemyKill);

    }
    private void FixedUpdate()
    {
        enemyAlive = enemySpawned;
    }
    public void RoundStart()
    {

        roundIncrease.Invoke();
        RoundAudio.PlayOneShot(NewRound);
        Debug.Log("The current round is: " + currentRound + " total enemys are" + currentRound * 7);
        totalEnemies = currentRound * 7;
        currentRoundState = RoundState.RoundBegin;  
        StartCoroutine(RoundWait());

    }
    public void RecivedOnEnemyKill()
    {
        enemyAlive -= 1;
        totalEnemies -= 1;
        if( totalEnemies <= 0  && currentRoundState == RoundState.RoundPlaying)
        {
            RoundEnd();
        }
    }
    public void OnenemySpawnedReceived()
    {
        enemySpawned += 1;
        Debug.Log("Onenemyspawned works" + enemySpawned);
    }
    public void RoundEnd()
    {
        currentRoundState = RoundState.RoundEnd;

        Invoke(nameof(RoundStart), 2f);// adds a delay of 5 seconds between rounds or atleast should. The wait is in the actual numerator so idk what the 2f is for honestly now that im looking back at it.

        currentRound++;
    }
    private IEnumerator RoundWait()
    {
        //Add in a audio play here.
        // audioSorce.PlayOneShot(newRoundClip);
        yield return new WaitForSeconds(5f);
        currentRoundState = RoundState.RoundPlaying;
        SpawnManager.Instance.StartSpawning(currentRound);
    }// new idea to just make a round wait for round start and then while playing have spawning.
}
