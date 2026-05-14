using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public bool IsUsed {  get; private set; }

    public void MarkUsed()
    {
        IsUsed = true;
    }
    public void ResetPoint()
    {
        IsUsed = false;
    }
}
