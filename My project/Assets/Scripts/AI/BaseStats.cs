using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseStats : MonoBehaviour, IDamageAble
{
    public static BaseStats Instance;


    SpawnManager spawnManager = new SpawnManager();
    public float zDamage;
    public float zHealth;
    public UnityEvent enemyKilled;
    public UnityEvent enemyHit;

    public bool IsAlive => zHealth > 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
       
    }

    public void Damage(float damage)// damage the zombie
    {
        zHealth -= damage;
        enemyHit.Invoke();
        Debug.Log("Remaing Zombie HP" + zHealth);
        if (zHealth <= 0)
        {        
            //spawnManager.EnemyKill();
            Debug.Log("Okay the zombie has died"); // go b ack through all the code and remember where the zombie dying is
            enemyKilled.Invoke();
            Destroy(gameObject);
            
        }
    }
}
