using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEditor.PackageManager;
using UnityEngine;

public class KnifeAttack : MonoBehaviour
{
    public float knifeRange = 2f;
    public int damage = 1;
    [SerializeField] private CinemachineCamera PlayerCamera;
    [SerializeField] Animator knifeAnimator;

    public void meleeSwing()
    {
        knifeAnimator.SetTrigger("MeleeHitSpot");
    }

    public void OnMeleeHit()
    {
        RaycastHit hit;
        if(Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out hit, knifeRange))
        {
            IDamageAble damageable = hit.transform.GetComponent<IDamageAble>();
            damageable?.Damage(damage);

            // this is where i should add points not in the enemy getting hit
        }
    }
    public void MeleeOver()
    {
        knifeAnimator.SetTrigger("MeleeOver");
    }
}
