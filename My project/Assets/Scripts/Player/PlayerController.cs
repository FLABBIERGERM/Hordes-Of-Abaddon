using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class PlayerController : MonoBehaviour
{
    [Header("PlayerInput")]
    private PlayerInputActions playerInputActions;
    private Vector2 movementInput;

    [SerializeField] private BaseMovement baseMovement;
    [SerializeField] private CharacterMovement characterMovement;
    [SerializeField] private CharacterInteractManager characterInteractManager;

    [SerializeField] private WeaponData gunData;
    [SerializeField] private Transform muzzle;
    [SerializeField] private Transform rifle;

    [SerializeField] private AudioClip gunNoise;

    float timeSinceLastShot;
    private bool CanShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        Debug.DrawRay(muzzle.position,gunData.maxDist * muzzle.forward, Color.red );
    }
    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    private void Start()
    {
        GameState.Instance.OnGamePaused.AddListener(OnGamePausedReceived);
        GameState.Instance.OnGamePaused.AddListener(OnGameResumedReceived);

    }

    private void OnEnable()
    {
        SwitchActionMap("Player");

        SubscribeInputActions();
    }

    private void OnDisable()
    {
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
                break;
            case "UI":
                playerInputActions.UI.Enable();
                Debug.Log("We are in the UI Action Map");

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
        //if(gunData.currentAmmo > 0)
        {
            if (CanShoot())
            {

                if (Physics.Raycast(muzzle.position, muzzle.forward, out RaycastHit hitInfo, gunData.maxDist))
                {

                    Debug.Log(hitInfo.transform.name);// tells me what its hitting may need it later and didnt want to remove it

                    IDamageAble damageable = hitInfo.transform.GetComponent<IDamageAble>();   
                    damageable?.Damage(gunData.damage);
                }
                //Debug.Log("Miss");
                gunData.currentAmmo--;
                timeSinceLastShot = 0;
                OnGunShot();
            }
        }
    }
    private void OnGunShot()
    {
        characterMovement.GunShotNoise();

        //Debug.Log("Gun has made it to the end of the if can shoot statement");
    }
    private IEnumerator Reload()
    {
        gunData.reloading = true; 

        yield return new WaitForSeconds(gunData.reloadTime);
        gunData.currentAmmo = gunData.magSize;
        gunData.reloading = false;
    }

    private void StartReload(InputAction.CallbackContext context)
    {
       // Debug.Log("Gun is reloading");
        StartCoroutine(Reload());   
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
        GameManager.Instance.TogglePause();
    }
    private void OnGamePausedReceived()
    {
        SwitchActionMap("UI");

    }
    private void OnGameResumedReceived()
    {
        SwitchActionMap("Player");
    }
}
