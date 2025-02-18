using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Death : MonoBehaviour
{
    [SerializeField] private GameState gameState;
    [SerializeField] private AudioSource myAudio = null;

    private UIDocument uiDocument;
    private VisualElement loseMenu;


    private Button restartGame;
    private Button mainMenu;
    
    private void Awake()
    {

    }

    void Start()
    {
        uiDocument = GetComponent<UIDocument>();

        loseMenu = uiDocument.rootVisualElement.Q<VisualElement>("lose-menu");

        loseMenu.style.display = DisplayStyle.None;
        GameState.Instance.OnPlayerLost.AddListener(RecivedOnGameLost);
        restartGame = loseMenu.Q<Button>("Restart-Game");
        mainMenu = loseMenu.Q<Button>("Main-Menu");
        restartGame.clicked += RestartGamePressed;
        mainMenu.clicked += MainMenuPressed;
    }
    private void OnDestroy()
    {
        restartGame.clicked -= RestartGamePressed;
        mainMenu.clicked -= MainMenuPressed;
    }

    private void RestartGamePressed()
    {
        loseMenu.style.display = DisplayStyle.None;
        UnityEngine.Cursor.visible = false;
        GameManager.Instance.GameStart();
        SceneManager.LoadScene("ActualMainScene");
    }
    private void MainMenuPressed()
    {
        SceneManager.LoadScene("MainMenu");
        loseMenu.style.display = DisplayStyle.None;
        UnityEngine.Cursor.visible = false;

    }

    private void DeathNoise()
    {
        myAudio.Play();
    }

    private void RecivedOnGameLost()
    {
        Debug.Log("Okay we have lost");

        loseMenu.style.display = DisplayStyle.Flex;
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        DeathNoise();
        Time.timeScale = 0f;

        // gonna have to implement a scene change that happens after x seconds
        // the scene will go back to main menu, i dont know how to actually reset a world yet, figure this out though and then change it to just be a lose menu into a main menu
        // instead of a lose menu into a scene change into a main menu to reset scnes.
    }
}
