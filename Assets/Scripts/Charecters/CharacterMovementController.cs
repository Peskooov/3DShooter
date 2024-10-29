using UnityEngine;

public class CharacterMovementController : MonoBehaviour
{
    [SerializeField] private CharacterMovement targetCharacterMovement;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        targetCharacterMovement.TargetDirectionControl = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (Input.GetButtonDown("Jump"))
            targetCharacterMovement.Jump();

        if (Input.GetKeyDown(KeyCode.C))
            targetCharacterMovement.Crouch();
        if (Input.GetKeyUp(KeyCode.C))
            targetCharacterMovement.UnCrouch();

        if (Input.GetKeyDown(KeyCode.LeftShift))
            targetCharacterMovement.Sprint();
        if (Input.GetKeyUp(KeyCode.LeftShift))
            targetCharacterMovement.UnSprint();

        if (Input.GetMouseButtonDown(1))
            targetCharacterMovement.Aiming();
        if (Input.GetMouseButtonUp(1))
            targetCharacterMovement.UnAiming();
    }
}
