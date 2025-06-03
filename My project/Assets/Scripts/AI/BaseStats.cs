using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class BaseStats : MonoBehaviour, IDamageAble
{
    public static BaseStats Instance;

    [SerializeField] Animator animator;
    [SerializeField] NavMeshAgent NavAgent;
    //public float aDamage;
    //public float aHealth;



    public float zDamage;
    public float zHealth;
    public UnityEvent enemyKilled;
    public UnityEvent enemyHit;
    public UnityEvent angelHalf;
    public bool dead = false;
    public bool IsAlive => zHealth > 0;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void Damage(float damage)// damage the enemy not the player.
    {
        zHealth -= damage;
        enemyHit.Invoke();
        Debug.Log("Remaing Zombie HP" + zHealth);
        if(zHealth <= (zHealth / 2) && (zHealth >0) && NavAgent.CompareTag("Angel") == true)
        {
            angelHalf.Invoke();
        }
        if (zHealth <= 0)
        {
            Debug.Log("Okay the zombie has died"); // go b ack through all the code and remember where the zombie dying is
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
