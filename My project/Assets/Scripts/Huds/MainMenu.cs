using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameState gameState;

    private VisualElement root;

    private Button StartGame;
    private Button creditButton;
    private Button QuitGame;
    private VisualElement Mainm;
    private UIDocument document;
    private VisualElement creditsz;

    private void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        document = GetComponent<UIDocument>();

        StartGame = root.Q<Button>("start-game");
        QuitGame = root.Q<Button>("quit-game");
        creditButton = root.Q<Button>("credit-button");

        creditsz = document.rootVisualElement.Q<VisualElement>("credits");
        Mainm = document.rootVisualElement.Q<VisualElement>("menu-container");

        StartGame.clicked += StartGamePressed;
        creditButton.clicked += CreditButtonPressed;
        QuitGame.clicked += QuitGamePressed;

        GameState.Instance.GameQuit.AddListener(ReceivedQuit);
    }

    private void OnDestroy()
    {
        creditButton.clicked -= CreditButtonPressed;
        StartGame.clicked -= StartGamePressed;
        QuitGame.clicked -= QuitGamePressed;
        GameState.Instance.GameQuit.RemoveListener(ReceivedQuit);

    }

    private void QuitGamePressed()
    {
        GameManager.Instance.GameQuit();
    }
    private void StartGamePressed()
    {
        Mainm.style.display = DisplayStyle.None;
        // either  change time to start game or change scene
    }
    private void CreditButtonPressed()
    {
        Mainm.style.display = DisplayStyle.None;
        GameManager.Instance.CreditsPlaying();
    }

    private void ReceivedQuit()
    {
        Application.Quit();
    }
}

