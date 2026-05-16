using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class SideArm : MonoBehaviour
{
    [Header("Weapon Type")]
    public string weaponType = "Gun";
    public bool fullAuto = false;

    [Header("Weapon Stats")]
    public float damage;
    public float maxDist;
    public int magSize;
    public float fireRate;
    public float reloadTime;

    public float nextShot;
     
    [Header("Weapon Updating Info")]
    public int totalAmmo;
    public int currentAmmo;
    public bool reloading;
    public float timeSinceLastShot;

    [Header("Particle systems")]

    // animation should probably have the gun fired particle bit in it rather than making one each time, fix this later.
    [SerializeField] private ParticleSystem onHitParticle;
    [SerializeField] private ParticleSystem gunFiredParticle;
    // this one bugs out for some reason
    [SerializeField] private ParticleSystem onObjectHitParticle;

    [Header("Audio and other")]
    [SerializeField] private AudioSource audioSource;
    public AudioClip[] myClips;
    [SerializeField] private Animator animator;
    public UnityEvent reloadingStarted;
    public UnityEvent reloadingFinished;
    [SerializeField] private CinemachineCamera playerCam;
    // makes a small function to return true or false checking if you are not reloading and if you are firing at a normal rate
    private bool CanShoot() => !reloading && currentAmmo >0 ; //&& timeSinceLastShot > 1f / (fireRate / 60f);


    public void fireWeapon()
    {
        //Debug.Log("So we are atleast getting to fire weapon");
        //Debug.Log("This is the check for the particles onhit: " + onHitParticle != null);
        //Debug.Log("This is the check for the particles gunfired: " + gunFiredParticle != null);
        //Debug.Log("This is the check for the particles objectHit: " + onObjectHitParticle != null);

        if (CanShoot())
        {
            animator.SetTrigger("FireWeapon");
           // Debug.Log("well we are able to shoot");
            audioSource.PlayOneShot(myClips[0]);
            if(Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out RaycastHit hitInfo, maxDist, 7))
            {
                if (hitInfo.collider.CompareTag("Zombie") || hitInfo.collider.CompareTag("Mutant"))
                {
                    Instantiate(onHitParticle, hitInfo.point, Quaternion.identity, hitInfo.collider.transform);
                    IDamageAble damageable = hitInfo.transform.GetComponent<IDamageAble>();
                    damageable?.Damage(damage,weaponType);
                }
            }
            else
            {
               // Debug.Log(hitInfo.transform.name.ToString());// tells me what its hitting may need it later and didnt want to remove it
               // no idea why but this returns an error which makes no sense to me weirrd

               // Instantiate(onObjectHitParticle, hitInfo.point, Quaternion.identity, hitInfo.collider.transform);

            }
            // add screen shake based on weapon
            currentAmmo--;
            nextShot = Time.time + fireRate;
        }

        if(currentAmmo <= 0 && reloading != true && totalAmmo >0)
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
        reloadingStarted.Invoke();
        reloading = true;
        animator.SetTrigger("ReloadingAnimation");
        audioSource.PlayOneShot(myClips[1]);

        yield return new WaitForSeconds(reloadTime);
        nothing();
        reloading = false;
        reloadingFinished.Invoke();
    }

    public void nothing()
    {

        int tempAmmo = magSize;
     // Debug.Log("Temp ammo count: " + tempAmmo);
        tempAmmo -= currentAmmo;
    //  Debug.Log("Temp ammo count after removing current ammo: " + tempAmmo);

        if(totalAmmo + tempAmmo <= magSize)
        {
       //   Debug.Log("TempAmmo after total is <= magsize: " + tempAmmo);
            tempAmmo = totalAmmo;
           //ebug.Log("TempAmmo after becoming totalAmmo: " + tempAmmo);
        }
        totalAmmo -= tempAmmo;

        currentAmmo += tempAmmo;
    }

   
}
