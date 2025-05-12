using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    [Header("PlayerInput")]
    private PlayerInputActions playerInputActions;
    private Vector2 movementInput;
    [SerializeField] private CinemachineCamera PlayerCamera;


    [SerializeField] private BaseMovement baseMovement;
    [SerializeField] private CharacterMovement characterMovement;
    [SerializeField] private CharacterInteractManager characterInteractManager;

    [SerializeField] private WeaponData gunData;
    [SerializeField] private Transform muzzle;// the muzzle is where the origin of the bullets comes from

    [SerializeField] private Transform rifle;
    [SerializeField] private Transform gunBarrel;

    [SerializeField] private AudioSource weaponAudioSource;
    [SerializeField] private AudioClip reloadingSound;

    [SerializeField] private ParticleSystem onHitParticle;
    [SerializeField] private ParticleSystem gunFiredParticle;
    [SerializeField] private ParticleSystem onObjectHitParticle;
    public UnityEvent reloadingStarted;
    public UnityEvent reloadingFinished;
    [SerializeField] private LayerMask ignoreMe;

    [SerializeField] private Transform casingSpawnPoint;
    [SerializeField] private GameObject bulletCasing;

    [SerializeField] private CinemachineShaking shaking;
    float timeSinceLastShot;
    private bool CanShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        Debug.DrawRay(muzzle.position,gunData.maxDist * muzzle.forward, Color.red );
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        playerInputActions = new PlayerInputActions();
    }

    private void Start()
    {
        GameState.Instance.OnGamePaused.AddListener(OnGamePausedReceived);
        GameState.Instance.OnGameResumed.AddListener(OnGameResumedReceived);
        GameState.Instance.GameStarts.AddListener(OnGameStartsReceived);

        if(gunData.reloading == true)
        {
            gunData.reloading = false;
        }
        if(gunData.currentAmmo < gunData.magSize)
        {
            gunData.currentAmmo = gunData.magSize;
        }

    }

    private void OnEnable()
    {
        SwitchActionMap("Player");

        SubscribeInputActions();
    }

    private void OnDisable()
    {
        Debug.Log("On disabled happened in player controller");
        UnsubscribeInputActions();
        SwitchActionMap();
    }


    private void SubscribeInputActions()
    {
        playerInputActions.Player.Move.started += MoveAction;
        playerInputActions.Player.Move.performed += MoveAction;
        playerInputActions.Player.Move.canceled += MoveAction;

        playerInputActions.Player.Jump.performed += JumpActionPerformed;
        playerInputActions.Player.Jump.performed += JumpActionCanceled;

        playerInputActions.Player.TogglePause.performed += TogglePauseActionPerformed;
        playerInputActions.UI.TogglePause.performed += TogglePauseActionPerformed;



        playerInputActions.Player.Shoot.performed += Shoot;
        playerInputActions.Player.Reload.performed += StartReload;

        playerInputActions.Player.Interact.performed += InteractActionPerformed;
       // playerInputActions.Player.Spray.performed += SprayActionPerformed;
    }
    private void UnsubscribeInputActions()
    {
        playerInputActions.Player.Move.started -= MoveAction;
        playerInputActions.Player.Move.performed -= MoveAction;
        playerInputActions.Player.Move.canceled -= MoveAction;

        playerInputActions.Player.Jump.performed -= JumpActionPerformed;
        playerInputActions.Player.Jump.performed -= JumpActionCanceled;

        playerInputActions.Player.TogglePause.performed -= TogglePauseActionPerformed;
        playerInputActions.UI.TogglePause.performed -= TogglePauseActionPerformed;

        playerInputActions.Player.Shoot.performed -= Shoot;
        playerInputActions.Player.Reload.performed -= StartReload;

        playerInputActions.Player.Interact.performed -= InteractActionPerformed;
        //playerInputActions.Player.Spray.performed -= SprayActionPerformed;
    }

    private void SwitchActionMap(string mapName = "")   
    {
        switch (mapName)
        {
            case "Player":
                playerInputActions.Player.Enable();
                Debug.Log("We are in the Player Action Map");
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                PlayerCamera.enabled = true;
                break;
            case "UI":
                playerInputActions.UI.Enable();
                Debug.Log("We are in the UI Action Map");

                PlayerCamera.enabled = false;
                Cursor.visible = true;
                
                Cursor.lockState = CursorLockMode.None;
                break;
            default:
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                break;
        }
    }


    private void Shoot(InputAction.CallbackContext context)
    {
        //Debug.Log("Gun is firing");
        if(gunData.currentAmmo > 0)
        {
            if (CanShoot())
            {

                if (Physics.Raycast(muzzle.position, muzzle.forward, out RaycastHit hitInfo, gunData.maxDist,7))
                {
                    if (hitInfo.collider.CompareTag("Zombie") || hitInfo.collider.CompareTag("Mutant"))
                    {
                        Instantiate(onHitParticle, hitInfo.point, Quaternion.identity,hitInfo.collider.transform);
                        Debug.Log(hitInfo.transform.name);// tells me what its hitting may need it later and didnt want to remove it

                        IDamageAble damageable = hitInfo.transform.GetComponent<IDamageAble>();
                        damageable?.Damage(gunData.damage);
                        
                    }
                    else
                    {
                        Instantiate(onObjectHitParticle, hitInfo.point, Quaternion.identity, hitInfo.collider.transform);
                        Debug.Log(hitInfo.transform.name);// tells me what its hitting may need it later and didnt want to remove it
                    }
                }
                //Debug.Log("Miss");
                shaking.ScreenShake(muzzle.forward);
                Instantiate(gunFiredParticle,gunBarrel.position, Quaternion.identity,gunBarrel);// went back and did more
                gunData.currentAmmo--;
                timeSinceLastShot = 0;
                OnGunShot();
            }
        }
        if(gunData.currentAmmo <= 0)
        {
            StartReload(context);
        }
    }
    private void OnGunShot()
    {
        characterMovement.GunShotNoise();
        characterMovement.GunRecoil();
        BCspawning();
        //CinemachineShaking.Instance.ShakeCamera(0.76f, 0.1f);
        //Debug.Log("Gun has made it to the end of the if can shoot statement");
    }

    private void BCspawning()
    {
        // Quaternion storageAtempt = new Quaternion(-90f, casingSpawnPoint.rotation.y, casingSpawnPoint.rotation.z, 0f);
        
        GameObject BulletCasing = Instantiate(bulletCasing, casingSpawnPoint);
         //BulletCasing.transform.rotation = Quaternion.Euler(-90f, casingSpawnPoint.rotation.y, casingSpawnPoint.rotation.z); // This is causing my rotation issues but is also the only way my rotations working.
  
        Rigidbody BCRB = BulletCasing.GetComponent<Rigidbody>();

       
        BCRB.velocity = BCRB.transform.TransformDirection(new Vector3(Random.Range(-3,-5f), Random.Range(3, 5f), 0.5f) );
        

        StartCoroutine(BulletDespawn(BulletCasing));
    }

    private IEnumerator BulletDespawn(GameObject BulletCasing)
    {
        yield return new WaitForSeconds(2f);
        Destroy(BulletCasing);
    }

    private IEnumerator Reload()
    {
        gunData.reloading = true;
        reloadingStarted.Invoke();
        yield return new WaitForSeconds(gunData.reloadTime);
        gunData.currentAmmo = gunData.magSize;
        gunData.reloading = false;
        reloadingFinished.Invoke();
    }

    private void StartReload(InputAction.CallbackContext context)
    {
        // Debug.Log("Gun is reloading");
        if(gunData.reloading == false && gunData.currentAmmo != gunData.magSize)
        {
            characterMovement.ReloadingAnimation();
            weaponAudioSource.PlayOneShot(reloadingSound);
            StartCoroutine(Reload());
        }
    }

    private void MoveAction(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
       // Debug.Log("We  are atempting the move action");
        baseMovement.SetMovementInput(movementInput);
    }
    private void JumpActionPerformed(InputAction.CallbackContext context)
    {
      //  Debug.Log("Well we are actually jumping..");

        baseMovement.Jump();
    }
    private void InteractActionPerformed(InputAction.CallbackContext context)
    {
        characterInteractManager.Interact();
    }
    private void JumpActionCanceled(InputAction.CallbackContext context)
    {
        baseMovement.CancelJump();
    }
    private void SprintActionPerformed(InputAction.CallbackContext context)
    {
        baseMovement.StartSprinting();
    }
    private void SprintActionCanceled(InputAction.CallbackContext context)
    {
        baseMovement.StopSprinting();
    }
    private void TogglePauseActionPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("We are pushing the pause button");
        GameManager.Instance.PauseGame();
    }
    private void OnGamePausedReceived()
    {
        SwitchActionMap("UI");

    }
    private void OnGameResumedReceived()
    {
        SwitchActionMap("Player");
    }
    private void OnGameStartsReceived()
    {
        if(gunData.currentAmmo < gunData.magSize)
        {
            gunData.currentAmmo = gunData.magSize;
        }
    }
}
