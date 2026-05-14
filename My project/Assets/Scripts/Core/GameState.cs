using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class GameState : MonoBehaviour
{
    private static GameState _instance;

    // has the mainmenu scenes name actually be main menu
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    public static GameState Instance
    {
        get { return _instance; }
    }

    public GameStatus CurrentGameStatus { get; private set; }

    public int playerHealth = 15;

    public UnityEvent OnGamePaused;
 
    public UnityEvent OnGameResumed;
    public UnityEvent Credits;
    public UnityEvent OnPlayerWin;
    public UnityEvent OnPlayerLost;
    public UnityEvent GameStarts;
    public UnityEvent GameQuit;

    // pings out damage taken this will be changed.
    public UnityEvent damageTaken;

    // makes sure its the one and only then calls resetgamestate then loads the scene.
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
           // DontDestroyOnLoad(gameObject); // dont need this at the moment as theres one level figure out full implementation of this at a later date.
        }
        else
        {
            Destroy(gameObject);
        }
        ResetGameState();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    // honestly forget i think its just checking if its paused and returning true / false based on it.
    public bool IsPaused => CurrentGameStatus == GameStatus.Paused;

    // this updates the game status from any of the conditions like paused or playing or dead.
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


    // this removes all the listeners reseting the game status.
    public void ResetGameState()
    {
        Debug.Log("Reseting Game state");
        //CurrentGameStatus = GameStatus.Paused;
        OnGameResumed.RemoveAllListeners();
        OnGamePaused.RemoveAllListeners();
        OnPlayerLost.RemoveAllListeners();

        OnPlayerWin.RemoveAllListeners();

        damageTaken.RemoveAllListeners();
        GameStarts.RemoveAllListeners(); 
    }

    // when called it checks if we go to the main menu in which case it resets the game state to have no listeners and if not just updates the game state to gamestart .
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == mainMenuSceneName)
        {
            ResetGameState();
        }
        else
        {
            UpdateGameStatus(GameStatus.GameStart);
        }
    }
}
// the gamestatuses
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

