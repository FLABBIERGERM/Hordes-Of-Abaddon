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
    [SerializeField] private GameObject angelEnemy;

    [SerializeField] public Transform playerToFollow;

    [SerializeField] public int NumOfEnemies;
    public float bruiserEnemyChance = 0.2f;
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
        int numOfEnemies = round *  NumOfEnemies; // change number later based on difficulty
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

            // notes
            // Holy fuck this may work and look semi nice but it screwes the pooch on anything i try to do if its getting a new position or a wait time for the enemies like my god
            // this new one where iwant to target a position a single instance the players at has to be registered here aswell for some un godly reason so i guess a charge has to be instant and then idk how to udpate it


            //Debug.Log("We are in the while loop of the enemies to spawn coroutine.");
            
            GameObject enemy = Instantiate(
                enemiesToSpawn.Dequeue(),
                spawnPoints[Random.Range(0, spawnPoints.Length)].position,
                Quaternion.identity);
            AiStateController stateController = enemy.GetComponent<AiStateController>();
            if (stateController != null && stateController.aiBlackboard != null)
            {
                stateController.aiBlackboard.chaseTarget = playerToFollow;
                stateController.aiBlackboard.chargeLocation = playerToFollow.position;
               // stateController.aiBlackboard.chargeOver = false;
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
            yield return new WaitForSeconds(1.5f);
        }
       // Debug.Log("Enemies remaining after coroutiennne" + enemiesRemaining);
        RoundManager.Instance.currentRoundState = RoundManager.RoundState.RoundPlaying;

    }


}
