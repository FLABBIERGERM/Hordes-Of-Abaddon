using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEditor.UIElements;
using UnityEngine.UI;
using UnityEngine.Splines;

public class HudScore : MonoBehaviour
{
    public static HudScore Instance { get; private set; }

    private const int scalez = 5;
    private UIDocument uiDocument;
    private VisualElement scoreBoard;
    //private VisualElement hitMarker;
    private VisualElement reticleMarker;
    private VisualElement AmmoContainer;
    private VisualElement HealthContainer;
    private VisualElement KillContainer;

    private Label scoreLabel;
    private Label roundLabel;
    private Label essenceLabel;
    private Label ammoCount;
    private Label reloadingLabel;
    private Label MagSize;
    private ProgressBar hpBar;

    public int totalAmmo;
    public int currentAmmo;
    private int roundNumber = 1;
    private int MaxHp;
    private int CurrentHp;

    private string currentMagAmmo;

   

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
        KillContainer = uiDocument.rootVisualElement.Q<VisualElement>("Kill-Counter");

        if (scoreBoard == null)
        {
            Debug.LogError("scoreBoard not found");
            return;
        }
        scoreLabel = KillContainer.Q<Label>("Score");
        if (scoreLabel == null)
        {
            Debug.LogError("Score Label Not found");
            return;
        }
        HealthContainer = uiDocument.rootVisualElement.Q<VisualElement>("Health-Bar");
        hpBar = HealthContainer.Q<ProgressBar>("Health-Bar");
        AmmoContainer = uiDocument.rootVisualElement.Q<VisualElement>("Ammo-Container");
        roundLabel = scoreBoard.Q<Label>("Round-Holder");
        essenceLabel = scoreBoard.Q<Label>("Essence");

        ammoCount = AmmoContainer.Q<Label>("Current-Ammo");
        MagSize = AmmoContainer.Q<Label>("Mag-Size");


        currentAmmo = gunData.currentAmmo;
        totalAmmo = gunData.magSize;
        MaxHp = GameManager.Instance.playerMaxHp;
        CurrentHp = MaxHp;
        reticleMarker = uiDocument.rootVisualElement.Q<VisualElement>("Reticle");
        reloadingLabel = reticleMarker.Q<Label>("Reloading-Holder");

        reloadingLabel.style.display = DisplayStyle.None;
        //hitMarker.style.display = DisplayStyle.None;
        scoreBoard.style.display = DisplayStyle.Flex; // this is un needed as it never leaves but i want to leave it to potentially change later.
        KillContainer.style.display = DisplayStyle.Flex;

        hpBar.highValue = MaxHp;
        hpBar.value = CurrentHp;

        

        //essenceLabel.style.top;
    }

    private void Start()
    {
        PlayerController.Instance.reloadingStarted.AddListener(ReloadingReceived);
        PlayerController.Instance.reloadingFinished.AddListener(ReloadingFinishedReceived);
        RoundManager.Instance.roundIncrease.AddListener(RoundUp);
        GameState.Instance.OnPlayerLost.AddListener(RecivedOnGameLost);
        GameState.Instance.OnGamePaused.AddListener(ReceivedOnGamePaused);
        GameState.Instance.OnGameResumed.AddListener(ReceivedOnGameResumed);
        //UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.lockState = CursorLockMode.Confined;

        
    }
    //private void Start() => BaseStats.Instance.enemyKilled.AddListener(RecivedOnEnemyKill);

    private void RecivedOnGameLost()
    {
        scoreBoard.style.display = DisplayStyle.Flex; // this is un needed as it never leaves but i want to leave it to potentially change later.
        //hitMarker.style.display = DisplayStyle.None;
        reloadingLabel.style.display = DisplayStyle.None;
        reticleMarker.style.display = DisplayStyle.None;
        AmmoContainer.style.display = DisplayStyle.None;
    }
    public void RegisterEnemy(BaseStats enemyStats)
    {
        enemyStats.enemyKilled.AddListener(RecivedOnEnemyKill);
        enemyStats.enemyHit.AddListener(RecivedOnEnemyHit);
    }
    public void FixedUpdate()
    {
        killHolder = kills;
        CurrentHp = GameManager.Instance.PlayerCurrentHP;
        hpBar.value = CurrentHp;
        AmmoUpdate();
        ammoCount.text = (currentAmmo.ToString()); // this is the one for current ammo that updates
        MagSize.text = (("/")+totalAmmo.ToString());
        scoreLabel.text = ("Kills:") + killHolder.ToString();
        roundLabel.text = roundNumber.ToString();
        essenceLabel.text = ("Essence:") + essence.ToString();
    }

    private void AmmoUpdate()
    {
        currentAmmo = gunData.currentAmmo;

        if (currentAmmo <= 15 )
        {
            // change color here
            ammoCount.style.color = Color.yellow;
            if (currentAmmo <= 6)
            {
                // change color here
                ammoCount.style.color = Color.red;
            }
        }
        if (currentAmmo >= 16)
        {
            // change color here
            ammoCount.style.color = Color.white;

        }
        currentMagAmmo = currentAmmo.ToString();

    }
    private void ReceivedOnGamePaused()
    {
        //hitMarker.style.display = DisplayStyle.None;
        reloadingLabel.style.display = DisplayStyle.None;
        reticleMarker.style.display = DisplayStyle.None;
        scoreBoard.style.display = DisplayStyle.None;
        hpBar.style.display = DisplayStyle.None;
    }
    private void ReceivedOnGameResumed()
    {
       // hitMarker.style.display = DisplayStyle.Flex;
        reticleMarker.style.display = DisplayStyle.Flex;
        scoreBoard.style.display = DisplayStyle.Flex;
        hpBar.style.display= DisplayStyle.Flex;
        if (gunData.reloading)
        {
            reloadingLabel.style.display = DisplayStyle.Flex;
        }

    }
    private void RecivedOnEnemyHit()
    {
        StartCoroutine(HitMarker());
        audioSource.PlayOneShot(hitNoise);
        Debug.Log("Enemy hit");
        essence += 25;
        ShowFloatingScore("+25");
;    }
    private void RecivedOnEnemyKill()
    {
        Debug.Log("We are killing and recieving");
        kills += 1;
        essence += 50;
        ShowFloatingScore("+50");
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
        reticleMarker.style.unityBackgroundImageTintColor = Color.red;
        reticleMarker.style.maxWidth = 60f;
        reticleMarker.style.maxHeight = 60f;

        yield return new WaitForSeconds(0.1f);
        
        reticleMarker.style.unityBackgroundImageTintColor = Color.black;
        reticleMarker.style.maxWidth = 50f;
        reticleMarker.style.maxHeight = 50f;
    }

    private IEnumerator FloatingScore(VisualElement VE, float duration)
    {

        float TimeElapsed = 0f;
        float intOpacity = VE.resolvedStyle.opacity;

        while(TimeElapsed < duration)
        {
            TimeElapsed += Time.deltaTime;
            float becomeClear = Mathf.Lerp(intOpacity, 0f, TimeElapsed / duration);
            VE.style.opacity = becomeClear;
            yield return null;
        }
        VE.RemoveFromHierarchy();
    }
    private void ShowFloatingScore(string text)
    {
        Label tempEssence = new Label(text);
        tempEssence.style.position = Position.Absolute;
        tempEssence.style.fontSize = 30;
        tempEssence.style.color = Color.red;
        tempEssence.style.unityFontStyleAndWeight = FontStyle.Bold;
        tempEssence.style.opacity = 1;

        VisualElement Parent = essenceLabel.parent;
        Parent.Add(tempEssence);

        var essenceBounds = essenceLabel.resolvedStyle;

        float maxX = essenceBounds.width - 50;
        float maxY =  essenceBounds.height - 450;

        float randomX = Random.Range(0f,Mathf.Max(0f,maxX));
        float randomY = Random.Range(0f, Mathf.Max(-125f, maxY));
        
        tempEssence.style.left = essenceBounds.left + randomX;
        tempEssence.style.top = essenceBounds.top + randomY;
        StartCoroutine(FloatingScore(tempEssence, 0.5f));
    }
}
