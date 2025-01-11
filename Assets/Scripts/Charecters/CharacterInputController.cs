using UnityEngine;

public class CharacterInputController : MonoBehaviour
{
    [SerializeField] private CharacterMovement targetCharacterMovement;
    [SerializeField] private ThirdPersonCamera targetCamera;
    [SerializeField] private PlayerShooter targetShooter;

    [SerializeField] private SpreadShootRig spreadShootRig;

    [SerializeField] private Vector3 aimingOffset;
    [SerializeField] private Vector3 defaultOffset;

    private void Start()
    {
        LockMouse();
    }

    public void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
                spreadShootRig.Spread();
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

    public void AssignCamera(ThirdPersonCamera thirdPersonCamera)
    {
        targetCamera = thirdPersonCamera;
        targetCamera.IsRotateTarget = false;
        targetCamera.SetTargetOffset(defaultOffset);
        targetCamera.SetTarget(targetCharacterMovement.transform);
    }
}