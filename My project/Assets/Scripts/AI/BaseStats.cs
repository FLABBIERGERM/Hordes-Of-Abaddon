using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStats : MonoBehaviour
{

    public float zDamage;
    public int zHealth;
    public int zMaxHealth;

    public bool IsAlive => currentZHP > 0;

    int currentZHP;
    public void Hurt(int damage)// damage the zombie
    {
        currentZHP = Mathf.Clamp(currentZHP - damage, 0, zMaxHealth);

        if (currentZHP > 0)
        {
            Debug.Log("Okay the zombie has died"); // go b ack through all the code and remember where the zombie dying is
            
        }
    }
}
