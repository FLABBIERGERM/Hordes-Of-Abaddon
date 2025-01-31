using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    [SerializeField] private Animator animatorM;
    [SerializeField] private Transform[] spawnPoints;

    [SerializeField] private GameObject enemyMonster;

    [SerializeField] public Transform playerToFollow;

   // private AIBlackBoard aiBlackboard;

    private Queue<GameObject> enemiesToSpawn = new Queue<GameObject>();

    private int enemiesRemaining = 0 ;

    private int totalEnemies = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void FixedUpdate()
    {
        //CheckOnEnemies();
    }
    public void StartSpawning(int round)
    {
        int numOfEnemies = round *  3; // change number later based on difficulty
        Debug.Log(" So we are doing StartSpawning and the num of enemies there should be is :" + numOfEnemies);
        for (int i = 0; i < numOfEnemies; i++) 
        { 
            enemiesToSpawn.Enqueue(enemyMonster);
        }
        totalEnemies = enemiesToSpawn.Count;
        StartCoroutine(SpawningEnemies());
    }
    private IEnumerator SpawningEnemies()
    {
        Debug.Log("We have started the spawning enemies Coroutine");
        while(enemiesToSpawn.Count > 0)
        {
            //Debug.Log("We are in the while loop of the enemies to spawn coroutine.");
            GameObject enemy = Instantiate(

                enemiesToSpawn.Dequeue(),
                spawnPoints[Random.Range(0,spawnPoints.Length)].position,
                Quaternion.identity);
            AiStateController stateController = enemy.GetComponent<AiStateController>();
            if (stateController != null && stateController.aiBlackboard != null)
            {
                stateController.aiBlackboard.chaseTarget = playerToFollow;
            }
            BaseStats enemyStats = enemy.GetComponent<BaseStats>();
            if(enemyStats != null && HudScore.Instance != null)
            {
                HudScore.Instance.RegisterEnemy(enemyStats);
            }
            animatorM.SetBool("Spawned", false);
            enemiesRemaining++;
            Debug.Log("Enemys remaining" + enemiesRemaining);
            yield return new WaitForSeconds(3f);
        }
        if(enemiesRemaining <= 0 || totalEnemies <=0)
        {
            Debug.Log("enemies died to fast good job!");

            RoundManager.Instance.EnemyDefeated();
        }

    }
    public void EnemyKill()
    {
        totalEnemies--;
        enemiesRemaining--;
        Debug.Log("New enemies remaining" + enemiesRemaining);
       // Debug.Log("Total Enemies" + totalEnemies);
        if (enemiesRemaining <= 0 && RoundManager.Instance.currentRoundState == RoundManager.RoundState.RoundPlaying)
        {
            RoundManager.Instance.EnemyDefeated();
        }
    }

}
