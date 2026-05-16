using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEditor;
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

    [Header("Player Movement and Interactmanager")]
    [SerializeField] private BaseMovement baseMovement;
    [SerializeField] private CharacterMovement characterMovement;
    [SerializeField] private CharacterInteractManager characterInteractManager;

    [Header("Player Weapon")]
    public bool fullyAuto = false;
    [SerializeField] private WeaponData gunData;
    [SerializeField] private Transform muzzle;// the muzzle is where the origin of the bullets comes from
    [SerializeField] private Transform rifle;
    [SerializeField] private Transform gunBarrel;


    [SerializeField] private KnifeAttack knifeAttack;
    //float timeSinceLastShot;

    [SerializeField] public GameObject primary;
    [SerializeField] private GameObject secondary;
    public GameObject currentActive;
    public bool suppressingFire = false;
    [Header("Player Weapon Audio")]
    [SerializeField] private AudioSource weaponAudioSource;
    [SerializeField] private AudioClip reloadingSound;

    [Header("Player Weapon particle effects")]
    [SerializeField] private ParticleSystem onHitParticle;
    [SerializeField] private ParticleSystem gunFiredParticle;
    [SerializeField] private ParticleSystem onObjectHitParticle;

    [SerializeField] private LayerMask ignoreMe;
    [Header("Player Weapon Casings")]
    [SerializeField] private Transform casingSpawnPoint;
    [SerializeField] private GameObject bulletCasing;

    [Header("Player Camera ")]
    [SerializeField] private CinemachineCamera PlayerCamera;
    [SerializeField] private CinemachineShaking shaking;

    
    //private bool CanShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);

 
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
        currentActive = secondary;
        primary.SetActive(false);

        // remove this last, since im no longer using a scriptable object its not longer trying to bind everything to that 1 so i dont beleive i will need this.
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

    private void Update()
    {
        if(fullyAuto  && suppressingFire )
        {
            SideArm sideArm = currentActive.GetComponent<SideArm>();
            //if ya do the top it fires all 8 bullets instantly
            //currentActive.GetComponent<SideArm>().fireWeapon();
            if (Time.time >= sideArm.nextShot)
            {
                sideArm.fireWeapon();

            }
        }
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

        playerInputActions.Player.Melee.performed += Melee_performed;
        playerInputActions.Player.Primary.performed += PrimaryEquiped;
        playerInputActions.Player.Secondary.performed += SecondaryEquiped;
        playerInputActions.Player.Shoot.performed += Shoot;
        playerInputActions.Player.Shoot.canceled += Shoot;
        playerInputActions.Player.Reload.performed += StartReload;

        playerInputActions.Player.Interact.performed += InteractActionPerformed;
       // playerInputActions.Player.Spray.performed += SprayActionPerformed;
    }

    private void Melee_performed(InputAction.CallbackContext context)
    {
        MeleeAttack();
        
    }

    private void MeleeAttack()
    {
        knifeAttack.meleeSwing();
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

        playerInputActions.Player.Melee.performed -= Melee_performed;
        playerInputActions.Player.Primary.performed -= PrimaryEquiped;
        playerInputActions.Player.Secondary.performed -= SecondaryEquiped;

        playerInputActions.Player.Shoot.performed -= Shoot;
        playerInputActions.Player.Shoot.canceled -= Shoot;

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


    private void PrimaryEquiped(InputAction.CallbackContext context)
    {
        if (primary != null)
        {
            SwapTo(primary, secondary);
            currentActive = primary;
            CheckIfFullauto();

        }
    }
    private void SecondaryEquiped(InputAction.CallbackContext context)
    {
        if(secondary != null)
        {
            SwapTo(secondary, primary);
            currentActive = secondary;
            CheckIfFullauto();
        }

    }
    private void SwapTo(GameObject active, GameObject inactive)
    {
        active.SetActive(true);
        inactive.SetActive(false);

    }


    private void Shoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (fullyAuto == true)
            {
                suppressingFire = true;
            }
            else
            {
                currentActive.GetComponent<SideArm>().fireWeapon();

            }

        }
        else if (context.canceled)
        {
            suppressingFire = false;
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
        GameObject BulletCasing = Instantiate(bulletCasing, casingSpawnPoint);
         //BulletCasing.transform.rotation = Quaternion.Euler(-90f, casingSpawnPoint.rotation.y, casingSpawnPoint.rotation.z); // This is causing my rotation issues but is also the only way my rotations working.
  
        Rigidbody BCRB = BulletCasing.GetComponent<Rigidbody>();

        BCRB.velocity = BCRB.transform.TransformDirection(new Vector3(Random.Range(-2f,-5f), Random.Range(-5f, 5f), 0.06f));
        BCRB.AddRelativeTorque(Random.Range(-5000, -15000f), Random.Range(-5000, -15000f), Random.Range(-5000, -15000f));
        
        StartCoroutine(BulletDespawn(BulletCasing));
    }

    private IEnumerator BulletDespawn(GameObject BulletCasing)
    {
        yield return new WaitForSeconds(2f);
        Destroy(BulletCasing);
    }

private void CheckIfFullauto()
    {
        fullyAuto = currentActive.GetComponent<SideArm>().fullAuto;
    }

    private void StartReload(InputAction.CallbackContext context)
    {
        currentActive.GetComponent<SideArm>().reloadingPublic();
    }

    private void MoveAction(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();

        baseMovement.SetMovementInput(movementInput);
    }
    private void JumpActionPerformed(InputAction.CallbackContext context)
    {
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
