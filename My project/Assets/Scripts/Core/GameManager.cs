using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    private float timeHit;
    private float RegenCD = 5.0f;
    public float essence;
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
        ResumeGame();
    }

    private void FixedUpdate()
    {
        CheckIfRegenAble();
    }

    private void CheckIfRegenAble()
    {
        if (Time.time >= timeHit + RegenCD)
        {
            PassiveRegen();
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
        timeHit = Time.time;
        Debug.Log("Took damage in gameManager is working");
        gameState.ChangePlayerHealth(HurtMe);
    }

    public void PassiveRegen() // add this based of time since last damage taken.
    {
        gameState.ChangePlayerHealth(1);
    }
    public void PauseGame()
    {
        bool didPause = gameState.UpdateGameStatus(GameStatus.Paused);
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
