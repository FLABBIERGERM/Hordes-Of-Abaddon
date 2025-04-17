using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    [SerializeField] private Animator  gunAnimations;
    public void RecoilEnd()
    {
        gunAnimations.SetTrigger("RecoilEnd");
    }
    // this may be worthless maybeeeeeeeeeee.
}
