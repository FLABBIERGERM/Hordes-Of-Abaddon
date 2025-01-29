using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class DamageOverlay : MonoBehaviour
{
    public Volume volume;
    private Vignette vignette;

    private void Start()
    {
        if (volume != null && volume.profile != null)
        {
            if (volume.profile.TryGet<Vignette>(out vignette))
            {
                vignette.active = true;
            }
            else
            {
                Debug.Log("Vignette not found");
            }
        }
        else
        {
            Debug.LogError("Volume is missing!");
        }
    }
    public void IncreaseVignette(float intensity)
    {
        if (vignette != null)
        {
            vignette.intensity.overrideState = true;
            vignette.intensity.value = Mathf.Clamp(vignette.intensity.value + intensity, 0f, 1f);
        }
    }
    public void DecreaseVignette(float intensity)
    {
        if (vignette != null)
        {
            vignette.intensity.overrideState = true;
            vignette.intensity.value = Mathf.Clamp(vignette.intensity.value - intensity, 0f, 1f);
        }
    }
}
