using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class HudScore : MonoBehaviour
{
    public static HudScore Instance { get; private set; }
    private UIDocument uiDocument;
    private VisualElement scoreBoard;
    private Label scoreLabel;
    private Label roundLabel;
    private Label essenceLabel;
    private int roundNumber = 1;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip gunNoise;
    private  float essence = 0;

    public int kills;
    private int killHolder;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        uiDocument = GetComponent<UIDocument>();
        if(uiDocument == null)
        {
            Debug.LogError("UI document not found in game object");
            return;
        }
        scoreBoard = uiDocument.rootVisualElement.Q<VisualElement>("Score-Holder");
        if (scoreBoard == null)
        {
            Debug.LogError("scoreBoard not found");
            return;
        }
        scoreLabel = scoreBoard.Q<Label>("Score");
        if (scoreLabel == null)
        {
            Debug.LogError("Score Label Not found");
            return;
        }
        roundLabel = scoreBoard.Q<Label>("Round-Holder");
        essenceLabel = scoreBoard.Q<Label>("Essence");  
        scoreBoard.style.display = DisplayStyle.Flex; // this is un needed as it never leaves but i want to leave it to potentially change later.
    }

    private void Start()
    {

        RoundManager.Instance.roundIncrease.AddListener(RoundUp);
    }
    //private void Start() => BaseStats.Instance.enemyKilled.AddListener(RecivedOnEnemyKill);

    public void RegisterEnemy(BaseStats enemyStats)
    {
        enemyStats.enemyKilled.AddListener(RecivedOnEnemyKill);
        enemyStats.enemyHit.AddListener(RecivedOnEnemyHit);
    }
    public void FixedUpdate()
    {
        killHolder = kills;
        scoreLabel.text = ("Kills:") + killHolder.ToString();
        roundLabel.text = ("Round: ") + roundNumber.ToString();
        essenceLabel.text = ("Essence: ") + essence.ToString();
    }

    private void RecivedOnEnemyHit()
    {
        audioSource.PlayOneShot(gunNoise);
        Debug.Log("Enemy hit");
        essence += 25;
    }
    private void RecivedOnEnemyKill()
    {
        Debug.Log("We are killing and recieving");
        kills += 1;
        essence += 50;
    }
    private void RoundUp()
    {
        roundNumber += 1;
    }
}
