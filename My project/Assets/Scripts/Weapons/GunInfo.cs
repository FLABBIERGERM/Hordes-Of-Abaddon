using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName ="Weapons/Gun")]
public class WeaponData : ScriptableObject
{
    public new string name;

    //shooting
    public float damage;
    public float maxDist;

    // reloading
    public int currentAmmo;
    public int magSize;
    public float fireRate;
    public float reloadTime;


    public bool fullAuto;

    //[HideInInspector]
    public bool reloading;


}
