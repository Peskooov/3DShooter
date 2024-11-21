using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;

    [Header("Movement")] 
    [SerializeField] private float accelerationRate;
    [SerializeField] private float rifleRunSpeed;
    [SerializeField] private float rifleSprintSpeed;
    [SerializeField] private float aimingWalkSpeed;
    [SerializeField] private float aimingRunSpeed;
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float climbSpeed;

    [Header("State")] [SerializeField] private float crouchHeight;
    [SerializeField] private int raycastDownDistance;
    [SerializeField] private float raycastForwardDistance;

    private bool isAiming;
    private bool isJump;
    private bool isCrouch;
    private bool isSprint;
    private bool isClimbing;
    private bool isClimbingEnd;

    private float baseCharacterHeight;
    private float baseCharacterHeightOffset;

    private float distanceToGround;

    private Vector3 directionControl;
    private Vector3 movementDirection;

    [HideInInspector] public Vector3 TargetDirectionControl;
    

    public bool IsCrouch => isCrouch;
    public bool IsClimbing => isClimbing;
    public bool IsClimbingEnd => isClimbingEnd;
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
        CheckLadder();
    }

    private void Move()
    {
        directionControl =
            Vector3.MoveTowards(directionControl, TargetDirectionControl, accelerationRate * Time.deltaTime);

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

        if (isClimbing)
        {
            if (IsGrounded || !isClimbingEnd)
            {
                movementDirection.y = climbSpeed * Input.GetAxis("Vertical");
                movementDirection.x = directionControl.x;
                movementDirection.z = directionControl.z;
            }
            else
            {
                movementDirection.y = climbSpeed * Input.GetAxis("Vertical");
                movementDirection.x = 0;
                movementDirection.z = 0;
            }
        }
        else
        {
            movementDirection += Physics.gravity * Time.deltaTime;
        }
        
        if (characterController.enabled)
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

    private void CheckLadder()
    {
        RaycastHit hit;

        Vector3 startRay = transform.position + Vector3.up * 0.1f;
        Vector3 direction = transform.forward;

        if (Physics.Raycast(startRay, direction, out hit, raycastForwardDistance))
        {
            if (hit.collider.GetComponent<Ladder>())
            {
                isClimbing = true;

                Vector3 secondRayStart = hit.point + direction * 0.1f;
                x = secondRayStart;

                if (Physics.Raycast(secondRayStart, direction, out hit, raycastForwardDistance))
                {
                    isClimbingEnd = true;
                }
                else
                {
                    isClimbingEnd = false;
                }
            }
        }
        else
        {
            isClimbing = false;
            isClimbingEnd = false;
        }
    }

    private Vector3 x;
    
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Vector3 startRay = transform.position + Vector3.up * 0.1f;
        Vector3 direction = transform.forward;
        
        Gizmos.color = Color.red;
        Gizmos.DrawRay(startRay, direction * raycastForwardDistance);
        Gizmos.DrawRay(x, direction * raycastForwardDistance);
    }
#endif
}