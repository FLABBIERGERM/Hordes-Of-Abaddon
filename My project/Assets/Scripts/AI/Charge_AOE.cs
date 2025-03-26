using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge_AOE: MonoBehaviour
{
    public float damage = 5.0f;
    public float duration = 1.0f;
    public string enemyTag = "Enemy";

    private float startTime;

    private void Start()
    {
        startTime = Time.time; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(enemyTag))
        {
            IDamageAble damageAble = other.GetComponent<IDamageAble>();
            if (damageAble != null)
            {
                damageAble.Damage(5);
            }
        }

    }

    private void Update()
    {
        if(Time.time - startTime >= duration)
        {
            Destroy(gameObject);
        }
    }
}
