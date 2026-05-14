using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class SideArm : MonoBehaviour
{
    public float damage;
    public float maxDist;

    public int totalAmmo;
    // reloading
    public int currentAmmo;
    public int magSize;
    public float fireRate;
    public float reloadTime;

    //[HideInInspector]
    public bool reloading;

    float timeSinceLastShot;

    [SerializeField] private CinemachineCamera playerCam;

    // animation should probably have the gun fired particle bit in it rather than making one each time, fix this later.
    [SerializeField] private ParticleSystem onHitParticle;
    [SerializeField] private ParticleSystem gunFiredParticle;
    [SerializeField] private ParticleSystem onObjectHitParticle;

    [SerializeField] private AudioSource audioSource;
    public AudioClip[] myClips;

    [SerializeField] private Animator animator;
    // makes a small function to return true or false checking if you are not reloading and if you are firing at a normal rate
    private bool CanShoot() => !reloading; //&& timeSinceLastShot > 1f / (fireRate / 60f);

    public void fireWeapon()
    {
        Debug.Log("So we are atleast getting to fire weapon");
        if (CanShoot())
        {
            animator.SetTrigger("FireWeapon");
            Debug.Log("well we are able to shoot");
            audioSource.PlayOneShot(myClips[0]);
            if(Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out RaycastHit hitInfo, maxDist, 7))
            {
                if (hitInfo.collider.CompareTag("Zombie") || hitInfo.collider.CompareTag("Mutant"))
                {
                    Instantiate(onHitParticle, hitInfo.point, Quaternion.identity, hitInfo.collider.transform);
                    IDamageAble damageable = hitInfo.transform.GetComponent<IDamageAble>();
                    damageable?.Damage(damage);
                }
            }
            else
            {
                 Debug.Log(hitInfo.transform.name);// tells me what its hitting may need it later and didnt want to remove it

                Instantiate(onObjectHitParticle, hitInfo.point, Quaternion.identity, hitInfo.collider.transform);

            }
            // add screen shake based on weapon

            currentAmmo--;
            timeSinceLastShot = 0;
        }

        if(currentAmmo <= 0 && reloading != true)
        {
            StartCoroutine(Reloadin());
        }
    }

    public void reloadingPublic()
    {
        if(currentAmmo < magSize && totalAmmo > 0)
        {
            StartCoroutine(Reloadin());
        }
    }
    private IEnumerator Reloadin()
    {
        reloading = true;
        animator.SetTrigger("ReloadingAnimation");
        audioSource.PlayOneShot(myClips[1]);

        yield return new WaitForSeconds(reloadTime);
        nothing();
        reloading = false;
    }

    public void nothing()
    {
        if(totalAmmo > magSize)
        {
            totalAmmo -= magSize;
            currentAmmo = magSize;
        }
        if(totalAmmo < magSize && totalAmmo > 0)
        {
            currentAmmo = totalAmmo;
            totalAmmo -= totalAmmo;
        }
    }

   
}
