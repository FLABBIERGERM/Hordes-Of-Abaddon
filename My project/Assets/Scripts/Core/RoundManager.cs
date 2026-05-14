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

    // this is just the audio section for a new round starting its audio source and then the music audio source and its songs
    [SerializeField] AudioClip NewRound;
    [SerializeField] AudioSource RoundAudio;
    [SerializeField] private AudioSource GameplayAudio;
    [SerializeField] private List<AudioClip> GamePlaySongs;

    // enemySpawned is the counter for enemies once they spawned, enemyAlive is the current enemies alive, currentRound is current round, and totalEnemies is the total enemies that should be spawned.
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


    // adds a listener for when an enemy is spawned from the spawn manager, sets the current round to 1 , starts the round and plays the next song.
    public void Start()
    {
        SpawnManager.Instance.enemySpawned.AddListener(OnenemySpawnedReceived);
        if(currentRound != 1)
        {
            currentRound = 1;
        }
        RoundStart();
        NextSong();

    }
    // this will register the enemy to add a listener for when the enemy dies from the stats.
    public void RegisterEnemy(BaseStats enemyStats)
    {
        enemyStats.enemyKilled.AddListener(RecivedOnEnemyKill);

    }
    // this updates making sure that enemyAlive is the same as enemySpawned and then calls nextsong.
    private void FixedUpdate()
    {
        enemyAlive = enemySpawned;
        NextSong();
    }

    // RoundStart function that gets called at the top of every round
    public void RoundStart()
    {
        // increases the round sending a signal for the hud and total enemy to spawn counters
        roundIncrease.Invoke();
        RoundAudio.PlayOneShot(NewRound);
        Debug.Log("The current round is: " + currentRound + " total enemys are" + currentRound * 10);
        totalEnemies = currentRound * 10;

        // sets the currentroundstate to roundbegin and starts the coroutine.
        currentRoundState = RoundState.RoundBegin;  
        StartCoroutine(RoundWait());

    }
    // catches the signal when an enemy dies
    public void RecivedOnEnemyKill()
    {
        // reduces enemies alive and total and checks if it should end the round.
        enemyAlive -= 1;
        totalEnemies -= 1;
        if( totalEnemies <= 0  && currentRoundState == RoundState.RoundPlaying)
        {
            RoundEnd();
        }
    }

    // catches when an enemy is spawned increasing the counter but not the total...
    public void OnenemySpawnedReceived()
    {
        enemySpawned += 1;
        Debug.Log("Onenemyspawned works" + enemySpawned);
    }

    // round end, just changes currentroundstate an increases the current round
    public void RoundEnd()
    {
        currentRoundState = RoundState.RoundEnd;

        Invoke(nameof(RoundStart), 2f);// adds a delay of 5 seconds between rounds or atleast should. The wait is in the actual numerator so idk what the 2f is for honestly now that im looking back at it.

        currentRound++;
    }
    // this makes a wait for 5 seconds between rounds and changes the roundstate to playing.
    private IEnumerator RoundWait()
    {
        //Add in a audio play here.
        // audioSorce.PlayOneShot(newRoundClip);
        yield return new WaitForSeconds(5f);
        currentRoundState = RoundState.RoundPlaying;
        SpawnManager.Instance.StartSpawning(currentRound);
    }// new idea to just make a round wait for round start and then while playing have spawning.



    void NextSong()
    {

        if (GameplayAudio.isPlaying != true)
        {
            int SongChoice = Random.Range(0, GamePlaySongs.Count);

            GameplayAudio.PlayOneShot(GamePlaySongs[SongChoice]);
            Debug.Log(("This is the song that is playing in game:") + SongChoice);// add this to t
            
        }

    }
}
