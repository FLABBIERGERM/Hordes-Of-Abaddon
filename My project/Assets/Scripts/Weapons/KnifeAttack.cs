using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEditor.PackageManager;
using UnityEngine;

public class KnifeAttack : MonoBehaviour
{
    public float knifeRange = 2f;
    public int damage = 1;
    public float pushForce = 4f;

    public string weaponType = "Knife";
    public float killpointValue = 25f;

    [SerializeField] private CinemachineCamera PlayerCamera;
    [SerializeField] Animator knifeAnimator;

    public void meleeSwing()
    {
        if(this.GetComponent<MeshRenderer>().enabled == false )
        {
            this.GetComponent<MeshRenderer>().enabled = true;
            this.GetComponent<MeshCollider>().enabled = true;
        }
        knifeAnimator.SetTrigger("MeleeHitSpot");
    }

    public void OnMeleeHit()
    {
        RaycastHit hit;
        if(Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out hit, knifeRange))
        {
            if(hit.collider.CompareTag("Zombie") || hit.collider.CompareTag("Mutant"))
            {
                Rigidbody rb = hit.collider.attachedRigidbody;
                if (rb != null)
                {
                    // this should add in a knock back we shall see if it causes issues or not
                    Vector3 dir = hit.transform.position - transform.position;
                    dir.y = 0;
                    rb.AddForce(dir.normalized * pushForce, ForceMode.Impulse);
                }
                IDamageAble damageable = hit.transform.GetComponent<IDamageAble>();
                damageable?.Damage(damage, weaponType);
            }


            // this is where i should add points not in the enemy getting hit
        }
    }

    public void MeleeOver()
    {
        knifeAnimator.SetTrigger("MeleeOver");
    }
}
