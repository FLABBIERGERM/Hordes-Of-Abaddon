using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Damage;
    public float bulletDespawn;

    private void start()
    {
        Destroy(gameObject, bulletDespawn);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var holder = other.gameObject.GetComponent<BaseStats>();

            if (holder != null)
            {
                holder.Damage(Damage);
                Destroy(gameObject);
            }
        }
    }

    public void ConfigureProjectile(int damageValue, float despawnValue)
    {
        Damage = damageValue;
        bulletDespawn = despawnValue;
    }
}
