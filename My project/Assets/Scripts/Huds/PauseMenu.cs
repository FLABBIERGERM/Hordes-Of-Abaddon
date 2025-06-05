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
    private Button fontChange;
    [SerializeField] private Font gothic;
    [SerializeField] private Font basic;
    private bool fonton = true;
    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        resumeButton = root.Q<Button>("resume-button");
        quitButton = root.Q<Button>("quit-button");
        fontChange = root.Q<Button>("Font_Change");
        fontChange.clicked += fontChangeClicked;
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
        fontChange.clicked -= fontChangeClicked;
        GameState.Instance.OnGamePaused.RemoveListener(ReceivedOnGamePaused);
        GameState.Instance.OnGameResumed.RemoveListener(ReceivedOnGameResumed);
    }

    private void ResumeButtonPressed()
    {
        GameManager.Instance.ResumeGame();

    }
    private void fontChangeClicked()
    {
        fonton = !fonton;
        Font selectedFont = fonton ? gothic : basic;
        root.style.unityFont = selectedFont;
        fontSetter(resumeButton, selectedFont);
        fontSetter(quitButton, selectedFont);
        fontSetter(fontChange, selectedFont);

    }
    private void fontSetter(Button button, Font font)
    {
        var label = button.Q<Label>();
        if (label != null)
        {
            label.style.unityFont = font;
        }

    }
    private void QuitButtonPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void ReceivedOnGamePaused()
    {
        root.style.visibility = Visibility.Visible;
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
 
    }

    private void ReceivedOnGameResumed()
    {
        root.style.visibility = Visibility.Hidden;
        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }
}
