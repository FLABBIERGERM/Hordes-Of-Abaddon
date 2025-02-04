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
    public UnityEvent enemySpawned;
    public UnityEvent enemyDefeated;

    //public static SpawnManager Instance
    //{
    //    get { return _instance; }
    //}
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
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

        Debug.Log("We have started the spawning enemies Coroutine" + totalEnemies);
        while (enemiesToSpawn.Count > 0)
        {
            //Debug.Log("We are in the while loop of the enemies to spawn coroutine.");
            GameObject enemy = Instantiate(
                enemiesToSpawn.Dequeue(),
                spawnPoints[Random.Range(0, spawnPoints.Length)].position,
                Quaternion.identity);
            AiStateController stateController = enemy.GetComponent<AiStateController>();
            if (stateController != null && stateController.aiBlackboard != null)
            {
                stateController.aiBlackboard.chaseTarget = playerToFollow;
            }
            // this is the area to add more to i think, adding in the total enemy count and then have it lower tracking it
            // or add in, inside of the round manager instead of spawn manager a way to check the rounds . Otherwise i need to track enemies better and i cant figure out why its not working correctly.
            BaseStats enemyStats = enemy.GetComponent<BaseStats>();
            if (enemyStats != null && HudScore.Instance != null)
            {
                HudScore.Instance.RegisterEnemy(enemyStats);
            }
            //animatorM.SetBool("Spawned", false);
            //enemySpawned.Invoke();
            enemiesRemaining++;
            //Debug.Log("Enemys remaining" + enemiesRemaining);
            yield return new WaitForSeconds(3f);
        }
       // Debug.Log("Enemies remaining after coroutiennne" + enemiesRemaining);
        RoundManager.Instance.currentRoundState = RoundManager.RoundState.RoundPlaying;

        //if (enemiesRemaining <= 0 || totalEnemies <=0)
        //{
        //    Debug.Log("enemies died to fast good job!");

        //    RoundManager.Instance.AllEnemysDefeated();
        //}

    }
    public void EnemyKill()
    {
        //enemyDefeated.Invoke();
        //totalEnemies--;
        //enemiesRemaining -= 1;
        //Debug.Log("New enemies remaining" + enemiesRemaining);
        //Debug.Log("Total Enemies" + totalEnemies);
        //if (enemiesRemaining <= 0 && RoundManager.Instance.currentRoundState == RoundManager.RoundState.RoundPlaying)
        //{
        //    RoundManager.Instance.AllEnemysDefeated();
        //}
    }

}
