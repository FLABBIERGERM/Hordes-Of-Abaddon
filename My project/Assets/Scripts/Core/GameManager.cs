using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    // this is the player getting hurt and having I frames section, this will get moved later.
    private float timeHit;
    private float ifTime = 0.5f;
    public bool canTakeDamage = true;

    //player currency.
    public float essence;

    // player healing and the damage overlay section
    private float RegenCD = 5.0f;
    public  int playerMaxHp = 15;
    public int PlayerCurrentHP;
    public DamageOverlay damageOverlay;

    // audio sources for the players hp and then the clips.
    [SerializeField] private AudioSource playerHPAudioSource;
    [SerializeField] private AudioSource playerHealing; // this is its own thing as the healing is very loud and im not working on a mixer currently.
    [SerializeField] private AudioSource UnderHalf;
    [SerializeField] private AudioSource HeavyBreathingSource;// i will need to go back in later once i get more knowledge on audio mixers and actually mix these properly instead of just having a bunch of em

    [SerializeField] private AudioClip playerHealingAudioClip;
    [SerializeField] private AudioClip getsHit;


    // this is for taking damage, aka shakes the camera when player gets hit.
    [SerializeField] private CinemachineShaking playerDShaking;

    public static GameManager Instance
    {
        get { return _instance; }
    }
    // makes a variable of gamestate
    private GameState gameState;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }
    // sets the players current hp to the max sets the gamestate variable to an instance of GameState and then calls resume game.
    private void Start()
    {

        PlayerCurrentHP = playerMaxHp;

        gameState = GameState.Instance;
        Debug.Log("Current HP:" + PlayerCurrentHP);
        ResumeGame();
    }

    // basically checks if we are not in the main menu and then if we are not it will check if you can regen hp or if you are dying.(low hp)
    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            CheckIfRegenAble();
            CheckIfDying();
        }
    }
    // this checks of the player is "dying" which means if they are under half hp.
    public void CheckIfDying()
    {
        if (PlayerCurrentHP <= 7)
        {
            if(UnderHalf.isPlaying!= true)
            {
                UnderHalf.Play();
            }
            if (HeavyBreathingSource.isPlaying != true)
            {
                HeavyBreathingSource.Play();
            }
            
        }
        if(PlayerCurrentHP > 7 )
        {
            if (UnderHalf.isPlaying == true)
            {
                UnderHalf.Stop();
            }
            if(HeavyBreathingSource.isPlaying == true)
            {
                HeavyBreathingSource.Stop();
            }
        }
    }

    // this is my coroutine for I frames.
    private IEnumerator Iframes()
    {
        yield  return new  WaitForSeconds(ifTime);
        canTakeDamage = true;
    }
    // checks on the players time since last hit and the regends cooldown and that the player is below their max hp then heals if so.
    public void CheckIfRegenAble()
    {
        //Debug.Log("Current Player Health in regen" + playerHP);
        //Debug.Log(Time.time);
        if ( Time.time >= timeHit + RegenCD  && PlayerCurrentHP < playerMaxHp)
        {
            Debug.Log("Okay we should be regening hp now");
            PassiveRegen();
        }
    }

    // this is how you as the player take damage or heal.
    public void ChangePlayerHealth(int healthDelta)
    {
        Debug.Log("Current Player Health" + PlayerCurrentHP);

        if (healthDelta < 0)
        {
            PlayerCurrentHP += healthDelta;
            playerHPAudioSource.PlayOneShot(getsHit);
            damageOverlay.IncreaseVignette(0.2f);
            playerDShaking.PlayerDamageShake(playerHPAudioSource.transform.forward);
            Debug.Log("Current Player Health" + PlayerCurrentHP);
        }
        if (healthDelta > 0)
        {
            PlayerCurrentHP += healthDelta;
            damageOverlay.DecreaseVignette(0.2f);
            playerHealing.PlayOneShot(playerHealingAudioClip);
            Debug.Log("Current Player Health" + PlayerCurrentHP);
        }
        if (PlayerCurrentHP <= 0)
        {

            GameManager.Instance.PlayerLost();
        }
    }

    // this checks of the credits are playing and shows them.
    public void CreditsPlaying()
    {
        bool didCredit = gameState.UpdateGameStatus(GameStatus.Credit);
        if (didCredit) {

            gameState.UpdateGameStatus(GameStatus.Credit);
        }
    }

    // this is how the player "takes" damage, basically if they get hit it calls this and passes the damage to the changeplayerHealth and in here calls the iframe coroutines.
    public void TookDamage(int HurtMe)
    {
        if(canTakeDamage == false)
        {
            Debug.Log("Cannot get hit for a moment");
            return;
        }
        if (canTakeDamage == true)
        {
            canTakeDamage = false;
            timeHit = Time.time;
            Debug.Log("Took damage in gameManager is working" + timeHit);
            ChangePlayerHealth(HurtMe);
            StartCoroutine(Iframes());
        }
    }

    // this is the passive regen basically just adds 1hp until hit or max.
    public void PassiveRegen() // add this based of time since last damage taken.
    {
       // if(PlayerCurrentHP < PlayerMaxHp)
        
            ChangePlayerHealth(1);// figure out how to make this a more stead regen rather than all or nothing.
           //ChangePlayerHealth(0.1f);
    }
    // this pauses the game which currently just makes the timescale 0 meaning everything physical freezes, the audio still players and so does the actual coroutine counters.
    public void PauseGame()
    {
        bool didPause = gameState.UpdateGameStatus(GameStatus.Paused);
        if (didPause)
        {
            Time.timeScale = 0;
        }
    }
    // just resumes the game making timescale 1.
    public void ResumeGame()
    {
        bool didResume = gameState.UpdateGameStatus(GameStatus.Playing);
        if (didResume)
        {
            Time.timeScale = 1;
            
        }
    }

    // failed toggle pause
    public void TogglePause()
    {
        if (gameState.IsPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    // these last one are pretty obvious.
    public void PlayerWon()
    {
        gameState.UpdateGameStatus(GameStatus.PlayerWon);
    }

    public void PlayerLost()
    {
        gameState.UpdateGameStatus(GameStatus.PlayerLost);
    }
    public void GameQuit()
    {
        gameState.UpdateGameStatus(GameStatus.GameQuit);// add the credits after this in this part haha.
    }
    public void GameStart()
    {
        gameState.UpdateGameStatus(GameStatus.GameStart);
    }
}
