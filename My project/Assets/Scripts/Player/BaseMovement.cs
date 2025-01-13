using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMovement : MonoBehaviour
{

    public float currentMaxSpeed = 7f;

    [SerializeField] protected float jumpForce = 6f;

    protected Vector2 movementInput;

    protected Vector3 movementDirection;

    [SerializeField] protected Animator animator;
    [SerializeField] protected CapsuleCollider capsuleCollider;
    [SerializeField] protected Rigidbody rigidbody;
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected Transform characterModel;



    public virtual void SetMovementInput(Vector2 moveInput)
    {
        movementInput = moveInput;
    }

    protected abstract void MoveCharacter();
    protected virtual void RotateCharacter()
    {
       
    }

    public virtual void Jump()
    {

    }

    public virtual void CancelJump()
    {

    }
    public virtual void StartSprinting()
    {

    }
    public virtual void StopSprinting()
    {

    }

    public virtual void CameraSwap()
    {

    }
    public virtual void Dance()
    {

    }
}
