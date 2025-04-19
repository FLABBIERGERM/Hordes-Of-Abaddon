using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class HudAmmo : MonoBehaviour
{
    private UIDocument uiDocument;
    private VisualElement ammoCounter;
    private Label ammoAmount;

    [SerializeField] private WeaponData gunData;

    public int totalAmmo;
    public int currentAmmo;

    private void Awake()    
    {
        uiDocument = GetComponent<UIDocument>();

        ammoCounter = uiDocument.rootVisualElement.Q<VisualElement>("Ammo-Holder");

        ammoAmount = ammoCounter.Q<Label>("Ammo");
        currentAmmo = gunData.currentAmmo;
        totalAmmo = gunData.magSize;

    }

    public void FixedUpdate()
    {
        AmmoUpdate();
        
        ammoAmount.text = ("Ammo:" + totalAmmo + ("/") + currentAmmo);
    }
    private void AmmoUpdate()
    {
        currentAmmo = gunData.currentAmmo;
    }
}
