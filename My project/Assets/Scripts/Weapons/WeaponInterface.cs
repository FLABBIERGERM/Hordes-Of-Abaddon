using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface WeaponInterface
{
    int CurrentAmmo { get; }
    int TotalAmmo {  get; }
    int MagSize {  get; }
    bool IsReloading {  get; }

    void ReloadAmmo();
}
