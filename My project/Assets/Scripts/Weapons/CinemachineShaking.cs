using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;

public class CinemachineShaking : MonoBehaviour
{
    public static CinemachineShaking Instance { get; private set; }

    private CinemachineCamera cinemachineCamera;
    private float shakeTimer;
    private float startingIntesnsity;
    private float shakeTimerTotal;

    [SerializeField] CinemachineImpulseSource CISource;
    [SerializeField] CinemachineImpulseSource PlayerHurtSource;
    [SerializeField] float powerAmount;

    private void Awake()
    {
        Instance = this;
        cinemachineCamera = GetComponent<CinemachineCamera>();

    }
    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin CinemachineNoise = cinemachineCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
        CinemachineNoise.AmplitudeGain = intensity;

        startingIntesnsity = intensity;
        shakeTimerTotal = time; 
        shakeTimer = time;
    }
    public void ScreenShake(Vector3 dir) // this is the easiest way to do this rather than my other code.
    {
        CISource.GenerateImpulseWithForce(powerAmount);
       // CISource.GenerateImpulseWithVelocity(dir);
    }
    public void PlayerDamageShake(Vector3 dir) 
    {
        PlayerHurtSource.GenerateImpulseWithVelocity(dir);
       // PlayerHurtSource.GenerateImpulseWithForce(powerAmount);
        
    }
    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                CinemachineBasicMultiChannelPerlin CinemachineNoise = cinemachineCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
                CinemachineNoise.AmplitudeGain = 0f;

                Mathf.Lerp(startingIntesnsity, 0f, 1-(shakeTimer / shakeTimerTotal));
            }
        }
    }
}
