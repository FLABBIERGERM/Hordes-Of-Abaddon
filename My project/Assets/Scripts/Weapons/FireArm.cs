using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireArm : MonoBehaviour
{
    [SerializeField] WeaponData gunData;


    public void Shoot()
    {
        Debug.Log("Gun is firing");
    }
}
