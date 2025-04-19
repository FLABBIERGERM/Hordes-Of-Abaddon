using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge_AOE: MonoBehaviour
{
    public int damage = 5;
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
            GameManager.Instance.TookDamage(-damage);
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
