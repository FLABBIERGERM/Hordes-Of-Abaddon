using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] WeaponData weaponData;

    private void Start()
    {
        PlayerController.Instance.reloadingFinished.AddListener(ReloadingFinishedReceived);
        PlayerController.Instance.reloadingStarted.AddListener(ReloadingReceived);

    }
    private void ReloadingFinishedReceived()
    {
        gameObject.SetActive(true);

    }
    private void ReloadingReceived()
    {
        gameObject.SetActive(true);

    }
    // this may be worthless maybeeeeeeeeeee.
}
