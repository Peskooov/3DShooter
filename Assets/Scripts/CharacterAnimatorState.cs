using System;
using UnityEngine;

public class CharacterAnimatorState : MonoBehaviour
{
    [SerializeField] private CharacterController targetCharacterController;
    [SerializeField] private CharacterMovement targetCharacterMovement;
    [SerializeField] private Animator targetAnimator;
    
    private void LateUpdate()
    {
        Vector3 movementSpeed = targetCharacterController.velocity;
        
        
        targetAnimator.SetFloat("NormalizeMovementX",movementSpeed.x / targetCharacterMovement.GetCurrentSpeed());
        targetAnimator.SetFloat("NormalizeMovementZ",movementSpeed.z / targetCharacterMovement.GetCurrentSpeed());
        
        targetAnimator.SetBool("IsGround", targetCharacterController.isGrounded);
        
        targetAnimator.SetBool("IsCrouch", targetCharacterMovement.IsCrouch);
        targetAnimator.SetBool("IsSprint", targetCharacterMovement.IsSprint);
        targetAnimator.SetBool("IsAiming", targetCharacterMovement.IsAiming);
        
        if(!targetCharacterController.isGrounded)
            targetAnimator.SetFloat("Jump", movementSpeed.y);
        
        Vector3 groundSpeed = targetCharacterController.velocity;
        groundSpeed.y = 0;
        targetAnimator.SetFloat("GroundSpeed", groundSpeed.magnitude);
        targetAnimator.SetFloat("DistanceToGround", targetCharacterMovement.DistanceToGround);
    }
}
