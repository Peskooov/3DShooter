using UnityEngine;
using UnityEngine.Serialization;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;

    [FormerlySerializedAs("AccelerationRate")]
    [Header("Movement")]
    [SerializeField] private float accelerationRate;
    [SerializeField] private float rifleRunSpeed;
    [SerializeField] private float rifleSprintSpeed;
    [SerializeField] private float aimingWalkSpeed;
    [SerializeField] private float aimingRunSpeed;
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float jumpSpeed;

    [Header("State")] 
    [SerializeField] private float crouchHeight;
    [SerializeField] private int raycastDownDistance;

    private bool isAiming;
    private bool isJump;
    private bool isCrouch;
    private bool isSprint;
    
    private float baseCharacterHeight;
    private float baseCharacterHeightOffset;

    private float distanceToGround;
    
    private Vector3 directionControl;
    private Vector3 movementDirection;
    
    [HideInInspector] 
    public Vector3 TargetDirectionControl;
    
    public bool IsCrouch => isCrouch;
    public bool IsSprint => isSprint;
    public bool IsAiming => isAiming;
    public bool IsJump => isJump;
    public float DistanceToGround => distanceToGround;
    public bool IsGrounded => distanceToGround < 0.01f;
    
    private void Start()
    {
        baseCharacterHeight = characterController.height;
        baseCharacterHeightOffset = characterController.center.y;
    }

    private void Update()
    {
        Move();

        UpdateDistanceToGround();
    }

    private void Move()
    {
        directionControl = Vector3.MoveTowards(directionControl, TargetDirectionControl, accelerationRate * Time.deltaTime);

        if (IsGrounded)
        {
            movementDirection = directionControl * GetCurrentSpeedByState();

            if (isJump)
            {
                movementDirection.y = jumpSpeed;
                isJump = false;
            }

            movementDirection = transform.TransformDirection(movementDirection);  
        }

        movementDirection += Physics.gravity * Time.deltaTime;

        characterController.Move(movementDirection * Time.deltaTime);
    }

    public void Jump()
    {
        if (IsGrounded == false) return;

        isJump = true;
    }

    public void Crouch()
    {
        if (!IsGrounded || isSprint) return;

        isCrouch = true;
        characterController.height = crouchHeight;
        characterController.center = new Vector3(0, characterController.height / 2, 0);
    }

    public void UnCrouch()
    {
        isCrouch = false;
        characterController.height = baseCharacterHeight;
        characterController.center = new Vector3(0, baseCharacterHeightOffset, 0);
    }

    public void Sprint()
    {
        if (!IsGrounded || isCrouch) return;

        isSprint = true;
    }

    public void UnSprint()
    {
        isSprint = false;
    }

    public void Aiming()
    {
        isAiming = true;
    }

    public void UnAiming()
    {
        isAiming = false;
    }

    private float GetCurrentSpeedByState()
    {
        if (isCrouch)
        {
            return crouchSpeed;
        }

        if (isAiming)
        {
            if (isSprint)
                return aimingRunSpeed;
            else
                return aimingWalkSpeed;
        }

        if (!isAiming)
        {
            if (isSprint)
                return rifleSprintSpeed;
            else
                return rifleRunSpeed;
        }

        return rifleRunSpeed;
    }

    public float GetCurrentSpeed() => GetCurrentSpeedByState();
    
    private void UpdateDistanceToGround()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -Vector3.up, out hit, raycastDownDistance))
            distanceToGround = Vector3.Distance(transform.position, hit.point);
    }
}