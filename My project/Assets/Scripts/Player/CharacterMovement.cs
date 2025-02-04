using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;
using Unity.IO.LowLevel.Unsafe;
public class CharacterMovement : BaseMovement
{
    [SerializeField] private float accelerationRate = 60f;
    [SerializeField] private float deccelerationRate = 30f;
    [SerializeField] private float maxWalkSpeed = 7f;
    [SerializeField] private float maxSprintSpeed = 15f;
    [SerializeField] private float maxVerticleSpeed = 25f;

    private bool isSprinting = false;

    [SerializeField] private int maxJumps = 2;

    private int currentJump = 0;

    [SerializeField] private float jumpCooldown = 0.25f;
    [SerializeField] private float airControllMultiplier = 0.4f;
    private bool readyToJump = true;

    [SerializeField] private float groundRotationRate = 10f;
    [SerializeField] private float airRotationRate = 3f;


    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private LayerMask enviormentLayerMask;

    private bool wasGroundedLastFrame = false;
    private bool isGrounded = false; // this is needed for animator

    [SerializeField] private Transform cameraTrasnform; // fix this word

    [SerializeField] private CinemachineCamera Camera1;

    [SerializeField] private AudioClip footStepSoundClip;

    [SerializeField] private AudioClip getsHit;

    [SerializeField] private AudioClip jumpNoise;

    [SerializeField] private AudioClip landingNoise;

    [SerializeField] private AudioClip gunNoise; // change into a animation once thing later
    [SerializeField] private AudioClip hitIndicator;
    private void FixedUpdate()
    {
        CheckIsGrounded();
        MoveCharacter();
        LimitVelocity();

    }
    private void Update()
    {
        CalculateCameraRelativeInput();

        RotateCharacter();

        animator.SetFloat("HorizontalSpeed", GetHorizontalRBVelocity().magnitude);// this are for animator
        animator.SetBool("IsFalling", (rigidbody.velocity.y < -0.01f));
    }

    private void Start()
    {
        currentMaxSpeed = maxWalkSpeed;
   
    }
    public void HitMarkerNoise()
    {
        audioSource.PlayOneShot(hitIndicator);
    }
    public void GunShotNoise()
    {
        audioSource.PlayOneShot(gunNoise);

    }
    public override void SetMovementInput(Vector2 moveInput)
    {
        base.SetMovementInput(moveInput);
    }

    void CalculateCameraRelativeInput()
    {
        Vector3 cameraForward = cameraTrasnform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();
        Vector3 cameraRight = cameraTrasnform.right;

        movementDirection = (cameraForward * movementInput.y) + (cameraRight * movementInput.x);

        if(movementDirection.sqrMagnitude > 1f)
        {
            movementDirection.Normalize();
        }
    }
    protected override void MoveCharacter()
    {
        if (movementDirection != Vector3.zero)
        {
            if (isGrounded)
            {
                rigidbody.AddForce(movementDirection * accelerationRate, ForceMode.Acceleration);
            }
            else
            {
                rigidbody.AddForce(movementDirection * (accelerationRate * airControllMultiplier), ForceMode.Acceleration);
            }
        }
        else if (isGrounded)
        {
            Vector3 currentVelocity = GetHorizontalRBVelocity();
            if(currentVelocity.magnitude > 0.05f)
            {
                Vector3 counteractDirection = currentVelocity.normalized * -1f;
                rigidbody.AddForce(counteractDirection * deccelerationRate, ForceMode.Acceleration);

            }
        }
    }

    public void PlayFootStep()
    {
        audioSource.PlayOneShot(footStepSoundClip);
    }
    
    protected override void RotateCharacter()
    {
        if (movementDirection != Vector3.zero)
        {
            if (isGrounded)
            {
                characterModel.forward = Vector3.Slerp(characterModel.forward, movementDirection.normalized, groundRotationRate * Time.deltaTime);
            }
            else
            {
                characterModel.forward = Vector3.Slerp(characterModel.forward, movementDirection.normalized, airRotationRate * Time.deltaTime);
            }
        }
    }

    private void LimitVelocity()
    {
        Vector3 currentVelocity = GetHorizontalRBVelocity();
        if(currentVelocity.sqrMagnitude > (currentMaxSpeed * currentMaxSpeed))
        {
            Vector3 counteractDirection = currentVelocity.normalized * -1f;
            float counteractAmount = currentVelocity.magnitude - currentMaxSpeed;
            rigidbody.AddForce(counteractDirection * counteractAmount, ForceMode.VelocityChange);
        }
        if(Mathf.Abs(rigidbody.velocity.y) > maxVerticleSpeed)
        {
            Vector3 counteractDirection = Vector3.up * Mathf.Sign(rigidbody.velocity.y) * 1f;
            float counteractAmount = Mathf.Abs(rigidbody.velocity.y) - maxVerticleSpeed;
            rigidbody.AddForce(counteractDirection * counteractAmount, ForceMode.VelocityChange);
        }
    }

    public override void Jump()
    {
        if(readyToJump &&(isGrounded || currentJump <= maxJumps))
        {
            animator.SetTrigger("Jump");
            audioSource.PlayOneShot(jumpNoise);
            currentJump += 1;
            float adjustedJumpForce = jumpForce - rigidbody.velocity.y;
            rigidbody.AddForce(Vector3.up * adjustedJumpForce, ForceMode.VelocityChange);
            readyToJump = false;
            StartCoroutine(JumpCooldownCoroutine());

        }
    }
    public override void CancelJump()
    {
       if(rigidbody.velocity.y > 0f)
        {
            rigidbody.AddForce(Vector3.down * (rigidbody.velocity.y * 0.5f), ForceMode.VelocityChange);
        }
    }
    private IEnumerator JumpCooldownCoroutine()
    {
        yield return new WaitForSeconds(jumpCooldown);
        readyToJump = true;
    }

    public override void StartSprinting()
    {
        isSprinting = true;
        currentMaxSpeed = maxSprintSpeed;

    }
    public override void StopSprinting()
    {
        isSprinting =false;
        currentMaxSpeed = maxWalkSpeed;
    }

    private void CheckIsGrounded()
    {
        wasGroundedLastFrame = isGrounded;
        RaycastHit hit;

        Vector3 p1 = transform.position + (Vector3.up * capsuleCollider.radius);
        Vector3 p2 = transform.position + (Vector3.up * (capsuleCollider.bounds.size.y - capsuleCollider.radius));

        isGrounded = Physics.CapsuleCast(
            p1,
            p2,
            capsuleCollider.radius,
            Vector3.down,
            out hit,
            groundCheckDistance,
            enviormentLayerMask
            );
        if (!isGrounded)
        {
            Collider[] colliders = Physics.OverlapSphere(p1, capsuleCollider.radius + groundCheckDistance, enviormentLayerMask);
            isGrounded = (colliders.Length > 0);
        }
        animator.SetBool("IsGrounded", isGrounded);
        if (!wasGroundedLastFrame && isGrounded)
        {
            audioSource.PlayOneShot(landingNoise);
            currentJump = 0;
        }
        else if(wasGroundedLastFrame && !isGrounded)
        {
            if(currentJump == 0)
            {
                currentJump = 1;
            }
        }
    }
    private Vector3 GetHorizontalRBVelocity()
    {
        return new Vector3(rigidbody.velocity.x, 0f, rigidbody.velocity.z);
    }

}
