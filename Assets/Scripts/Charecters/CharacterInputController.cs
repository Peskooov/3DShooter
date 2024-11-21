using System.Collections.Generic;
using UnityEngine;

public class CharacterInputController : MonoBehaviour
{
    [SerializeField] private CharacterMovement targetCharacterMovement;
    [SerializeField] private EntityActionCollector targetActionCollector;
    [SerializeField] private ThirdPersonCamera targetCamera;
    [SerializeField] private PlayerShooter targetShooter;

    [SerializeField] private Vector3 aimingOffset;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
            targetCharacterMovement.TargetDirectionControl =
                new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        
        targetCamera.RotationControl = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        if (targetCharacterMovement.TargetDirectionControl != Vector3.zero || targetCharacterMovement.IsAiming)
            targetCamera.IsRotateTarget = true;
        else
            targetCamera.IsRotateTarget = false;

        if (Input.GetKey(KeyCode.E))
        {
            List<EntityContextAction> actionsList = targetActionCollector.GetAction<EntityContextAction>();

            for (int i = 0; i < actionsList.Count; i++)
            {
                actionsList[i].StartAction();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
            targetCharacterMovement.Jump();

        if (Input.GetKeyDown(KeyCode.C))
            targetCharacterMovement.Crouch();
        if (Input.GetKeyUp(KeyCode.C))
            targetCharacterMovement.UnCrouch();

        if (Input.GetKeyDown(KeyCode.LeftShift))
            targetCharacterMovement.Sprint();
        if (Input.GetKeyUp(KeyCode.LeftShift))
            targetCharacterMovement.UnSprint();

        if (Input.GetMouseButton(0))
        {
            if (targetCharacterMovement.IsAiming)
            {
                targetShooter.Shoot();
            }
        }

        if (Input.GetMouseButton(1))
        {
            targetCharacterMovement.Aiming();
            targetCamera.SetTargetOffset(aimingOffset);
        }

        if (Input.GetMouseButtonUp(1))
        {
            targetCharacterMovement.UnAiming();
            targetCamera.SetDefaultOffset();
        }
    }
}