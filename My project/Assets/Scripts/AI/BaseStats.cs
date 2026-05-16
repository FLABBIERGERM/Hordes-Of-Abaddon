using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class BaseStats : MonoBehaviour, IDamageAble
{
    public UnityEvent<string> weaponKill;

    public static BaseStats Instance;

    [SerializeField] Animator animator;
    [SerializeField] NavMeshAgent NavAgent;



    //already half is just for the boss checking basically if it goes into a next state based on hp.
    public bool alreadyHalf = false;
    public UnityEvent angelHalf;
    //base hp and damage enemies deal
    public float zDamage;
    public float zHealth;

    //killed and hit events that ping out when a enemy gets hit / dies.
    public UnityEvent enemyKilled;
    public UnityEvent enemyHit;
    

    
    public bool dead = false;
    // is alive should be the main thing comparing if the enemy is dead or not
    public bool IsAlive => zHealth > 0;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void Damage(float damage,string weaponType)// damage the enemy not the player.
    {
        zHealth -= damage;
        enemyHit.Invoke();
        animator.SetTrigger("GetsShot");
       // Debug.Log("Remaing Zombie HP" + zHealth);


        if(zHealth <= 500 && NavAgent.CompareTag("Angel") == true )
        {
            if(!alreadyHalf) {
                alreadyHalf = true;
                angelHalf.Invoke();
            }

        }
        if (zHealth <= 0)
        {
            //Debug.Log("This is the weapon that got the kill" + weaponType);
            weaponKill.Invoke(weaponType);
           // Debug.Log("Okay the zombie has died"); // go b ack through all the code and remember where the zombie dying is
            enemyKilled.Invoke();
            dead = true;
            if (dead == true)
            {
                animator.SetBool("Dying",true);
                NavAgent.speed = 0;
                NavAgent.GetComponent<Collider>().enabled = false;
                NavAgent.isStopped = true;
                //gameObject.GetComponent<CapsuleCollider>().enabled = false; 
            }
        }
    }
    public void Dead()
    {
        Destroy(gameObject);
    }

}
