using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameState gameState;
    private UIDocument document;

    private Button StartGame;
    private Button creditButton;
    private Button QuitGame;
    private Button HTP;
    private Button Back;

    private VisualElement root;
    private VisualElement Mainm;
    private VisualElement creditsz;
    private VisualElement HTPve;
    private VisualElement currentVE;

    [SerializeField] private AudioSource mainMenuAudio;
    [SerializeField] private List<AudioClip> mainMenuSongs;


    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        document = GetComponent<UIDocument>();

        StartGame = root.Q<Button>("Start-Game");
        QuitGame = root.Q<Button>("Quit-Game");
        creditButton = root.Q<Button>("Credits");
        HTP = root.Q<Button>("How-To-Play");
        Back = root.Q<Button>("Back-Button");

        creditsz = document.rootVisualElement.Q<VisualElement>("Credit-Page");
        Mainm = document.rootVisualElement.Q<VisualElement>("Main-Menu-Holder");
        HTPve = document.rootVisualElement.Q<VisualElement>("How-Too-Play");
        currentVE = Mainm;

        StartGame.clicked += StartGamePressed;
        creditButton.clicked += CreditButtonPressed;
        QuitGame.clicked += QuitGamePressed;
        HTP.clicked += HTPpressed;
        Back.clicked += BackPressed;
        creditsz.style.display = DisplayStyle.None;
        HTPve.style.display = DisplayStyle.None;

        GameState.Instance.GameQuit.AddListener(ReceivedQuit);
        
        if (currentVE == Mainm)
        {
            Back.style.display = DisplayStyle.None;
        }
    }

    private void OnDestroy()
    {
        creditButton.clicked -= CreditButtonPressed;
        StartGame.clicked -= StartGamePressed;
        QuitGame.clicked -= QuitGamePressed;
        HTP.clicked -= HTPpressed;
        Back.clicked -= BackPressed;
        GameState.Instance.GameQuit.RemoveListener(ReceivedQuit);

    }

     void Update()
    {
        NextSong();
    }
    void NextSong()
    {

        if(mainMenuAudio.isPlaying != true)
        {
            int SongChoice = Random.Range(0, mainMenuSongs.Count);

            mainMenuAudio.PlayOneShot(mainMenuSongs[SongChoice]);
            Debug.Log(("This is the song that is play:") + SongChoice);// add this to t
        }

    }
    private void QuitGamePressed()
    {
        GameManager.Instance.GameQuit();
    }
    private void BackPressed()
    {
        currentVE.style.display = DisplayStyle.None;
        Mainm.style.display = DisplayStyle.Flex;
        Back.style.display = DisplayStyle.None;
        currentVE = Mainm;
    }
    private void StartGamePressed()
    {
        Mainm.style.display = DisplayStyle.None;
        GameManager.Instance.GameStart();
        SceneManager.LoadScene("ActualMainScene");
        // either  change time to start game or change scene
    }
    private void HTPpressed()
    {
        Mainm.style.display = DisplayStyle.None;
        Back.style.display= DisplayStyle.Flex;
        currentVE = HTPve;
        HTPve.style.display = DisplayStyle.Flex;
    }
    private void CreditButtonPressed()
    {
        Mainm.style.display = DisplayStyle.None;

        Back.style.display = DisplayStyle.Flex;
        currentVE = creditsz;

        creditsz.style.display = DisplayStyle.Flex;
        
        //GameManager.Instance.CreditsPlaying();
    }
    
    private void ReceivedQuit()
    {
        Application.Quit();
    }
}

