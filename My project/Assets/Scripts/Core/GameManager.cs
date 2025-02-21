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

    private const float RegenCD = 5.0f;
    private int PlayerMaxHp = 15;
    private int playerHP = 15;
    private int PlayerCurrentHP;
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
        gameState = GameState.Instance;
        PlayerCurrentHP = playerHP;
        ResumeGame();
    }

    private void FixedUpdate()
    {
        CheckIfRegenAble();
    }

    private IEnumerator Iframes()
    {
        yield  return new  WaitForSeconds(ifTime);
        canTakeDamage = true;
    }
    private void CheckIfRegenAble()
    {
        if (Time.time >= timeHit + RegenCD && PlayerCurrentHP < PlayerMaxHp)
        {
            PassiveRegen();
        }
    }
    public void ChangePlayerHealth(int healthDelta)
    {
        Debug.Log("Current Player Health" + playerHP);

        if (healthDelta < 0)
        {
            playerHP += healthDelta;
            damageOverlay.IncreaseVignette(0.2f);
        }
        if (healthDelta > 0)
        {
            playerHP += healthDelta;
            damageOverlay.DecreaseVignette(0.2f);
        }
        else
        {
            Debug.LogWarning("Some how damage done to player is equal to 0");
        }
        if (playerHP <= 0)
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
            return;
        }
        if (canTakeDamage == true)
        {
            canTakeDamage = false;
            timeHit = Time.time;
            Debug.Log("Took damage in gameManager is working");
            ChangePlayerHealth(HurtMe);
            StartCoroutine(Iframes());
        }
    }

    public void PassiveRegen() // add this based of time since last damage taken.
    {
        ChangePlayerHealth(1);
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
