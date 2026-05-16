using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    // current spawn points available this becomes changed based on room_manager aka whichever rooms are unlocked.
    // Should also implement a way for it being the closest spawn points not just all of them.
    //[SerializeField] private Transform[] spawnPoints;
    private List<Transform> activeSpawnPoints;
    private Dictionary<Transform, float> spawnCooldowns = new();
    // enemyMonster is the basic enemy while bruisers are the special enemies.
    [SerializeField] private GameObject enemyMonster;
    [SerializeField] private GameObject bruiserEnemy;
    // angelenemy is the large angel that is the "boss"
    [SerializeField] private GameObject angelEnemy;
    // player to follow is the player themselves.
    [SerializeField] public Transform playerToFollow;
    //NumOfenemies is the set amount of enemies that will be the basis for spawning example " numenemies = 5 so 5xround"
    [SerializeField] public int NumOfEnemies;
    // this is the base chance of a bruiser spawninging, its also changeable in the inspector.
    public float bruiserEnemyChance = 0.2f;

    [SerializeField] private LayerMask enemyLayer;
    
    // this is the que for the enemies to spawn once the game is started, basically it loads what enemies to spawn into a que 
    // think like it saying okay brusier was selected 5 times in a row so the next 5 spawns in brusier or any combo. 
    private Queue<GameObject> enemiesToSpawn = null;

    // enemiesRemaining was only a way to see enemies alive i think.
    private int enemiesRemaining = 0 ;

    // total enemies is the total enemies to spawn, it was used to more or less see if it was spawning the correct amount.
    private int totalEnemies = 0;

    // enemyspawned is listened to in round manatger.
    public UnityEvent enemySpawned;

    // doesnt have any script listening for this it seems.
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
        StartCoroutine(CDCleanup());
    }


        
    public void StartSpawning(int round)
    {
        enemiesToSpawn = new Queue<GameObject>();
        int numOfEnemies = round *  NumOfEnemies; // change number later based on difficulty
       // Debug.Log(" So we are doing StartSpawning and the num of enemies there should be is :" + numOfEnemies);

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
        activeSpawnPoints = Room_Manager.Instance.GetActiveSpawnPoints();
        //Debug.Log("We have started the spawning enemies Coroutine" + totalEnemies);
        //basically while there are enemiestospawn it spawns the enemy.

        while (enemiesToSpawn.Count > 0)
        {
            // notes
            // Holy fuck this may work and look semi nice but it screwes the pooch on anything i try to do if its getting a new position or a wait time for the enemies like my god
            // this new one where iwant to target a position a single instance the players at has to be registered here aswell for some un godly reason so i guess a charge has to be instant and then idk how to udpate it
            Transform spawnPoint = GetBestSpawnpoint();

            if (spawnPoint == null)
            {
                yield return null;
                continue;
            }
            GameObject enemy = Instantiate(
                enemiesToSpawn.Dequeue(),
                spawnPoint.position,
                Quaternion.identity);

            spawnCooldowns[spawnPoint] = Time.time + 5f;

            AiStateController stateController = enemy.GetComponent<AiStateController>();

            if (stateController != null && stateController.aiBlackboard != null)
            {
                stateController.aiBlackboard.chaseTarget = playerToFollow;
                stateController.aiBlackboard.chargeLocation = playerToFollow.position;
               
            }


            // this is the area to add more to i think, adding in the total enemy count and then have it lower tracking it
            // or add in, inside of the round manager instead of spawn manager a way to check the rounds . Otherwise i need to track enemies better and i cant figure out why its not working correctly.
            BaseStats enemyStats = enemy.GetComponent<BaseStats>();
            if (enemyStats != null && HudScore.Instance != null)
            {
                HudScore.Instance.RegisterEnemy(enemyStats);
                RoundManager.Instance.RegisterEnemy(enemyStats);             
            }
            
            enemiesRemaining++;
            yield return new WaitForSeconds(1.2f);
        }
       // Debug.Log("Enemies remaining after coroutiennne" + enemiesRemaining);
        RoundManager.Instance.currentRoundState = RoundManager.RoundState.RoundPlaying;

    }
    private IEnumerator CDCleanup()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            foreach (var key in new List<Transform>(spawnCooldowns.Keys))
            {
                if (spawnCooldowns[key] < Time.time)
                {
                    spawnCooldowns.Remove(key);
                }
            }
        }
    }
    //private Transform GetBestSpawnpoint()
    //{

    //    if(activeSpawnPoints == null || activeSpawnPoints.Count == 0)
    //    {
    //        return null;
    //    }
    //    Transform best = null;
    //    float bestScore = float.MinValue;
    //    foreach (var point in activeSpawnPoints)
    //    {
    //        if (spawnCooldowns.ContainsKey(point)&& spawnCooldowns[point] >Time.time)
    //        {
    //            continue;
    //        }
    //        // anti clump
    //        if(Physics.CheckSphere(point.position, 2.5f, enemyLayer))
    //        {
    //            continue;
    //        }
    //        float dist = Vector3.Distance(playerToFollow.position, point.position);

    //        float score = dist + Random.Range(0f, 4f);
    //        if (score > bestScore)
    //        {
    //            bestScore = score;
    //            best = point;
    //        }
    //    }
    //    if (best == null)
    //    {
    //        return activeSpawnPoints[Random.Range(0, activeSpawnPoints.Count)];
    //    }
    //    return best;
    //}

    private Transform GetBestSpawnpoint()
    {

        if (activeSpawnPoints == null || activeSpawnPoints.Count == 0)
        {
            return null;
        }

        List<Transform> valid = activeSpawnPoints.FindAll(p => !spawnCooldowns.ContainsKey(p) || spawnCooldowns[p] <= Time.time);
        valid = valid.FindAll(p => !Physics.CheckSphere(p.position, 2.5f, enemyLayer));
        if (valid.Count > 0)
        {
            return valid[Random.Range(0, valid.Count)];
        }
        return null;
    }
}
