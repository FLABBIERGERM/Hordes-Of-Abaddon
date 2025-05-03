using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    private float timeHit;
    private float ifTime = 0.5f;
    public bool canTakeDamage = true;

    public float essence;

    private float RegenCD = 5.0f;
    public  int playerMaxHp = 15;
    public int PlayerCurrentHP;
    public DamageOverlay damageOverlay;

    [SerializeField] private AudioSource playerHPAudioSource;
    [SerializeField] private AudioSource playerHealing; // this is its own thing as the healing is very loud and im not working on a mixer currently.
    [SerializeField] private AudioSource UnderHalf;
    [SerializeField] private AudioSource HeavyBreathingSource;// i will need to go back in later once i get more knowledge on audio mixers and actually mix these properly instead of just having a bunch of em

    [SerializeField] private AudioClip playerHealingAudioClip;
    [SerializeField] private AudioClip getsHit;
    [SerializeField] private AudioClip HeavyBreathing;

    // this is for ttaking damage
    [SerializeField] private CinemachineShaking playerDShaking;

    public static GameManager Instance
    {
        get { return _instance; }
    }

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
    private void Start()
    {

        PlayerCurrentHP = playerMaxHp;

        gameState = GameState.Instance;
        Debug.Log("Current HP:" + PlayerCurrentHP);
        ResumeGame();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            CheckIfRegenAble();
            CheckIfDying();
        }

    }

    public void CheckIfDying()
    {
        if (PlayerCurrentHP <= 7)
        {
            if(UnderHalf.isPlaying!= true)
            {
                UnderHalf.Play();
            }
            
        }
        if(PlayerCurrentHP > 7 && UnderHalf.isPlaying == true)
        {
            UnderHalf.Stop();
        }
    }
    private IEnumerator Iframes()
    {
        yield  return new  WaitForSeconds(ifTime);
        canTakeDamage = true;
    }
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
    public void CreditsPlaying()
    {
        bool didCredit = gameState.UpdateGameStatus(GameStatus.Credit);
        if (didCredit) {

            gameState.UpdateGameStatus(GameStatus.Credit);
        }
    }
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

    public void PassiveRegen() // add this based of time since last damage taken.
    {
       // if(PlayerCurrentHP < PlayerMaxHp)
        
            ChangePlayerHealth(1);// figure out how to make this a more stead regen rather than all or nothing.
           //ChangePlayerHealth(0.1f);
    }
    public void PauseGame()
    {
        bool didPause = gameState.UpdateGameStatus(GameStatus.Paused);
        if (didPause)
        {
            Time.timeScale = 0;
        }
    }
    public void ResumeGame()
    {
        bool didResume = gameState.UpdateGameStatus(GameStatus.Playing);
        if (didResume)
        {
            Time.timeScale = 1;
            
        }
    }
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
