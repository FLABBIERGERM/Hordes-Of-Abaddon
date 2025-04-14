using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    private float timeHit;
    private float ifTime = 0.5f;
    public bool canTakeDamage = true;

    public float essence;

    private float RegenCD = 5.0f;
    private int playerMaxHp = 15;
    public int PlayerCurrentHP;
    public DamageOverlay damageOverlay;

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
        //ResumeGame();
    }

    private void Update()
    {
        CheckIfRegenAble();
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
          
            damageOverlay.IncreaseVignette(0.2f);
            Debug.Log("Current Player Health" + PlayerCurrentHP);
        }
        if (healthDelta > 0)
        {
            PlayerCurrentHP += healthDelta;
            damageOverlay.DecreaseVignette(0.2f);
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
