using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

   // [SerializeField] private Animator animatorM;


    [SerializeField] private Transform[] spawnPoints;

    [SerializeField] private GameObject enemyMonster;
    [SerializeField] private GameObject bruiserEnemy;

    [SerializeField] public Transform playerToFollow;

    public float bruiserEnemyChance = 0.5f;
    // private AIBlackBoard aiBlackboard;

    private Queue<GameObject> enemiesToSpawn = null;

    private int enemiesRemaining = 0 ;

    private int totalEnemies = 0;
    public UnityEvent enemySpawned;
    public UnityEvent enemyDefeated;

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
        enemiesToSpawn = new Queue<GameObject>();
        int numOfEnemies = round *  3; // change number later based on difficulty
        Debug.Log(" So we are doing StartSpawning and the num of enemies there should be is :" + numOfEnemies);
        for (int i = 0; i < numOfEnemies; i++) 
        {
            GameObject enemyToSpawn = Random.value < bruiserEnemyChance ? bruiserEnemy : enemyMonster;
            enemiesToSpawn.Enqueue(enemyToSpawn);
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
                RoundManager.Instance.RegisterEnemy(enemyStats);
            }
            //enemySpawned.Invoke();
            enemiesRemaining++;
            yield return new WaitForSeconds(3f);
        }
       // Debug.Log("Enemies remaining after coroutiennne" + enemiesRemaining);
        RoundManager.Instance.currentRoundState = RoundManager.RoundState.RoundPlaying;

    }


}
