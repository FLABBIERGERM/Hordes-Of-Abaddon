using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance { get; private set; }
    public enum RoundState { RoundBegin, RoundPlaying, RoundEnd}

    public RoundState currentRoundState;

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
        DontDestroyOnLoad(gameObject);
    }
    public void Start()
    {
        SpawnManager.Instance.enemySpawned.AddListener(OnenemySpawnedReceived);

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
        Debug.Log("The current round is: " + currentRound + " total enemys are" + currentRound * 3);
        totalEnemies = currentRound * 3;
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

        Invoke(nameof(RoundStart), 2f);// adds a delay of 5 seconds between rounds or atleast should.

        currentRound++;
    }
    private IEnumerator RoundWait()
    {
        yield return new WaitForSeconds(5f);
        currentRoundState = RoundState.RoundPlaying;
        SpawnManager.Instance.StartSpawning(currentRound);
    }// new idea to just make a round wait for round start and then while playing have spawning.
}
