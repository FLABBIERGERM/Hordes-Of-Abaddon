using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    private VisualElement root;
    private Button resumeButton;
    private Button quitButton;
    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        resumeButton = root.Q<Button>("resume-button");
        quitButton = root.Q<Button>("quit-button");

        resumeButton.clicked += ResumeButtonPressed;
        quitButton.clicked += QuitButtonPressed;

        GameState.Instance.OnGamePaused.AddListener(ReceivedOnGamePaused);
        GameState.Instance.OnGameResumed.AddListener(ReceivedOnGameResumed);

        ReceivedOnGameResumed();
    }

    private void OnDestroy()
    {
        resumeButton.clicked -= ResumeButtonPressed;
        quitButton.clicked -= QuitButtonPressed;

        GameState.Instance.OnGamePaused.RemoveListener(ReceivedOnGamePaused);
        GameState.Instance.OnGameResumed.RemoveListener(ReceivedOnGameResumed);
    }

    private void ResumeButtonPressed()
    {
        GameManager.Instance.ResumeGame();
    }

    private void QuitButtonPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void ReceivedOnGamePaused()
    {
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        root.style.visibility = Visibility.Visible;
    }

    private void ReceivedOnGameResumed()
    {
        root.style.visibility = Visibility.Hidden;
    }
}
