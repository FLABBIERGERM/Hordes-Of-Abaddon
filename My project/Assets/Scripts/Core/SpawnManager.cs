using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    [SerializeField] private Animator animatorM;
    [SerializeField] private Transform[] spawnPoints;

    [SerializeField] private GameObject enemyMonster;


    private Queue<GameObject> enemiesToSpawn = new Queue<GameObject>();

    private int enemiesRemaining = 0;



    private void Awake()
    {
        Instance = this;
    }


    public void StartSpawning(int round)
    {
        int numOfEnemies = round * 10; // change number later based on difficulty
        Debug.Log(" So we are doing StartSpawning and the num of enemies there should be is :" + numOfEnemies);
        for (int i = 0; i < numOfEnemies; i++) 
        { 
            enemiesToSpawn.Enqueue(enemyMonster);
        }
        StartCoroutine(SpawningEnemies());
    }

    private IEnumerator SpawningEnemies()
    {
        Debug.Log("We have started the spawning enemies Coroutine");
        while(enemiesToSpawn.Count > 0)
        {
            Debug.Log("We are in the while loop of the enemies to spawn coroutine.");
            GameObject enemy = Instantiate(
                enemiesToSpawn.Dequeue(),
                spawnPoints[Random.Range(0,spawnPoints.Length)].position,
                Quaternion.identity);

            animatorM.SetBool("Spawned", false);
            enemiesRemaining++;
            yield return new WaitForSeconds(1f);
        }
        RoundManager.Instance.currentRoundState = RoundManager.RoundState.RoundPlaying;
    }

    private void EnemyKill()
    {
        enemiesRemaining--;

        if(enemiesRemaining <= 0 && RoundManager.Instance.currentRoundState == RoundManager.RoundState.RoundPlaying)
        {
            RoundManager.Instance.EnemyDefeated();
        }
    }
}
