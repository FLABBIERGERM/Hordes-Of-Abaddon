using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class GameState : MonoBehaviour
{
    private static GameState _instance;

    [SerializeField] private string mainMenuSceneName = "MainMenu";

    public static GameState Instance
    {
        get { return _instance; }
    }

    public GameStatus CurrentGameStatus { get; private set; }

    private int playerHealth = 3;

    public UnityEvent OnGamePaused;
 
    public UnityEvent OnGameResumed;
    public UnityEvent Credits;
    public UnityEvent OnPlayerWin;
    public UnityEvent OnPlayerLost;
    public UnityEvent GameStarts;
    public UnityEvent GameQuit;


    public UnityEvent damageTaken;
    public DamageOverlay damageOverlay;


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        ResetGameState();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public bool IsPaused => CurrentGameStatus == GameStatus.Paused;

    public bool UpdateGameStatus(GameStatus newGameStatus)
    {
        if (newGameStatus == CurrentGameStatus) { return false; }

        CurrentGameStatus = newGameStatus;
        switch (newGameStatus)
        {
            case GameStatus.Playing:
                OnGameResumed.Invoke();
                break;
            case GameStatus.Paused:
                OnGamePaused.Invoke();
                break;
            case GameStatus.PlayerWon:
                OnPlayerWin.Invoke();
                break;
            case GameStatus.PlayerLost: 
                OnPlayerLost.Invoke();
                break;
            case GameStatus.Credit:
                Credits.Invoke();
                break;
            case GameStatus.GameStart:
                GameStarts.Invoke();
                break;
            case GameStatus.GameQuit:
                GameQuit.Invoke();
                break;

            default:
                Debug.LogError("Unhandled Game status this should not happen.");
                break;
        }
        return true;
    }

    public void ChangePlayerHealth(int healthDelta)
    {
        playerHealth += healthDelta;
        if(healthDelta < 0)
        {
            damageOverlay.IncreaseVignette(0.2f);
        }
        if (healthDelta > 0)
        {
            damageOverlay.DecreaseVignette(0.2f);
        }
        if(playerHealth <= 0)
        {
            GameManager.Instance.PlayerLost();
        }
    }
    public void ResetGameState()
    {
        Debug.Log("Reseting Game state");
        CurrentGameStatus = GameStatus.Paused;



        OnGameResumed.RemoveAllListeners();
        OnGamePaused.RemoveAllListeners();
        OnPlayerLost.RemoveAllListeners();

        OnPlayerWin.RemoveAllListeners();

        damageTaken.RemoveAllListeners();
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == mainMenuSceneName)
        {
            ResetGameState();
        }
    }
}
public enum GameStatus
{
    Playing,
    Paused,
    PlayerWon,
    PlayerLost,
    Credit,
    GameStart,
    GameQuit
}

