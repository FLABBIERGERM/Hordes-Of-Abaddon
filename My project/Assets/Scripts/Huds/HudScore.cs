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
    private VisualElement hitMarker;
    private VisualElement reticleMarker;


    private Label scoreLabel;
    private Label roundLabel;
    private Label essenceLabel;
    private Label ammoCount;
    private Label reloadingLabel;

    public int totalAmmo;
    public int currentAmmo;
    private int roundNumber = 1;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip hitNoise;
    [SerializeField] private WeaponData gunData;
    public  float essence = 0;

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
        ammoCount = scoreBoard.Q<Label>("Ammo");
        hitMarker = uiDocument.rootVisualElement.Q<VisualElement>("Hit-Marker");

        currentAmmo = gunData.currentAmmo;
        totalAmmo = gunData.magSize;
        reticleMarker= uiDocument.rootVisualElement.Q<VisualElement>("Reticle");
        reloadingLabel = reticleMarker.Q<Label>("Reloading-Holder");

        reloadingLabel.style.display = DisplayStyle.None;
        hitMarker.style.display = DisplayStyle.None;
        scoreBoard.style.display = DisplayStyle.Flex; // this is un needed as it never leaves but i want to leave it to potentially change later.


    }

    private void Start()
    {
        PlayerController.Instance.reloadingStarted.AddListener(ReloadingReceived);
        PlayerController.Instance.reloadingFinished.AddListener(ReloadingFinishedReceived);
        RoundManager.Instance.roundIncrease.AddListener(RoundUp);
        GameState.Instance.OnPlayerLost.AddListener(RecivedOnGameLost);
        //UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.lockState = CursorLockMode.Confined;

    }
    //private void Start() => BaseStats.Instance.enemyKilled.AddListener(RecivedOnEnemyKill);

    private void RecivedOnGameLost()
    {
        scoreBoard.style.display = DisplayStyle.Flex; // this is un needed as it never leaves but i want to leave it to potentially change later.
        hitMarker.style.display = DisplayStyle.None;
        reloadingLabel.style.display = DisplayStyle.None;
        reticleMarker.style.display = DisplayStyle.None;
    }
    public void RegisterEnemy(BaseStats enemyStats)
    {
        enemyStats.enemyKilled.AddListener(RecivedOnEnemyKill);
        enemyStats.enemyHit.AddListener(RecivedOnEnemyHit);
    }
    public void FixedUpdate()
    {
        killHolder = kills;
        AmmoUpdate();
        ammoCount.text = ("Ammo:" + totalAmmo + ("/" )+ currentAmmo);
        scoreLabel.text = ("Kills:") + killHolder.ToString();
        roundLabel.text = ("Round: ") + roundNumber.ToString();
        essenceLabel.text = ("Essence: ") + essence.ToString();
    }

    private void AmmoUpdate()
    {
        currentAmmo = gunData.currentAmmo;
    }

    private void RecivedOnEnemyHit()
    {
        StartCoroutine(HitMarker());
        audioSource.PlayOneShot(hitNoise);
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
    private void ReloadingFinishedReceived()
    {
        reloadingLabel.style.display = DisplayStyle.None;

    }

    private void ReloadingReceived()
    {
        reloadingLabel.style.display = DisplayStyle.Flex;
    }
    private IEnumerator HitMarker()
    {
        hitMarker.style.display = DisplayStyle.Flex;
        yield return new WaitForSeconds(0.1f); 
        //yield return new WaitForEndOfFrame();
        hitMarker.style.display = DisplayStyle.None ;


    }
}
