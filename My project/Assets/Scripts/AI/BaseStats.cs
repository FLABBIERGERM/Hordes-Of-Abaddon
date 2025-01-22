using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStats : MonoBehaviour, IDamageAble
{
    SpawnManager spawnManager = new SpawnManager();
    public float zDamage;
    public float zHealth;
   // public float zMaxHealth;

    public bool IsAlive => zHealth > 0;

    //float currentZHP;
    private void Start()
    {
        //currentZHP = zMaxHealth;
    }
    public void Damage(float damage)// damage the zombie
    {
        //currentZHP = Mathf.Clamp(currentZHP - damage, 0, zMaxHealth);
        zHealth -= damage;
        Debug.Log("Remaing Zombie HP" + zHealth);
        if (zHealth <= 0)
        {
            spawnManager.EnemyKill();
            Debug.Log("Okay the zombie has died"); // go b ack through all the code and remember where the zombie dying is
            Destroy(gameObject);
        }
    }
}
