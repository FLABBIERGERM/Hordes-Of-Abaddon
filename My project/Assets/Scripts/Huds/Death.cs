using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Death : MonoBehaviour
{
    [SerializeField] private AudioSource myAudio = null;

    private UIDocument uiDocument;
    private VisualElement loseMenu;

    private void Awake()
    {
        uiDocument = GetComponent<UIDocument>();

        loseMenu = uiDocument.rootVisualElement.Q<VisualElement>("lose-menu");

        loseMenu.style.display = DisplayStyle.None;
    }

    void Start()
    {
        GameState.Instance.OnPlayerLost.AddListener(RecivedOnGameLost);
    }

    private void DeathNoise()
    {
        myAudio.Play();
    }

    private void RecivedOnGameLost()
    {
        Debug.Log("Okay we have lost");

        //loseMenu.style.display = DisplayStyle.Flex;
       // DeathNoise();
        Time.timeScale = 0f;

        // gonna have to implement a scene change that happens after x seconds
        // the scene will go back to main menu, i dont know how to actually reset a world yet, figure this out though and then change it to just be a lose menu into a main menu
        // instead of a lose menu into a scene change into a main menu to reset scnes.
    }
}
