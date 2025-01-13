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

    float timeSinceLastShot;
    private bool CanShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate/60f);

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
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
        playerInputActions.Player.Reload.performed += Reload;
       // playerInputActions.Player.CameraSwap.performed += ToggleCameraPerformed;

       // playerInputActions.Player.Dance.performed += DancingTime;
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
        playerInputActions.Player.Reload.performed -= Reload;

        // playerInputActions.Player.CameraSwap.performed -= ToggleCameraPerformed;

        //playerInputActions.Player.Dance.performed -= DancingTime;
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
    private void DancingTime(InputAction.CallbackContext context)
    {
        baseMovement.Dance();
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        Debug.Log("Gun is firing");
        if(gunData.currentAmmo > 0)
        {
            if (CanShoot())
            {
                if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, gunData.maxDist))
                {
                    Debug.Log(hitInfo.transform.name);// tells me what its hitting
                }

                gunData.currentAmmo--;
                timeSinceLastShot = 0;
                OnGunShot();
            }
        }
    }
    private void OnGunShot()
    {

    }

        private void Reload(InputAction.CallbackContext context)
    {
        Debug.Log("Gun is reloading");
    }
    private void MoveAction(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
        Debug.Log("We  are atempting the move action");
        baseMovement.SetMovementInput(movementInput);
    }
    private void JumpActionPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Well we are actually jumping..");

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
