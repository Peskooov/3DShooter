using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    [SerializeField] private float sensetive;
    [SerializeField] private float changeOffsetRate;
    [SerializeField] private float rotateTargetLerpRate;

    [Header("Rotation Limit")] 
    [SerializeField] private float minLimitY;
    [SerializeField] private float maxLimitY;

    [Header("Distance")]
    [SerializeField] private float distance;
    [SerializeField] private float minDistance;
    [SerializeField] private float distanceLerpRate;
    [SerializeField] private float distanceOffsetFromCollisionHit;
    
    [HideInInspector] public bool IsRotateTarget;
    [HideInInspector] public Vector2 RotationControl;

    private float deltaRotationX;
    private float deltaRotationY;

    private float currentDistance;
    
    private Vector3 targetOffset;
    private Vector3 defaultOffset;

    private void Start()
    {
        targetOffset = offset;
        defaultOffset = offset;
    }

    private void Update()
    {
        //Calculate position & rotation
        deltaRotationX += RotationControl.x * sensetive;
        deltaRotationY += RotationControl.y * -sensetive;

        deltaRotationY = ClampAngle(deltaRotationY, minLimitY, maxLimitY);

        offset = Vector3.MoveTowards(offset, targetOffset, changeOffsetRate * Time.deltaTime);

        Quaternion finalRotation = Quaternion.Euler(deltaRotationY, deltaRotationX, 0);
        Vector3 finalPosition = target.position - (finalRotation * Vector3.forward * distance);
        finalPosition = AddLocalOffset(finalPosition);
        
        // Calculate current distance
        float targetDistance = distance;

        RaycastHit hit;

        Debug.DrawLine(target.position + new Vector3(0, offset.y, 0), finalPosition, Color.red);

        if (Physics.Linecast(target.position + new Vector3(0, offset.y, 0), finalPosition, out hit))
        {
            float distanceToHit = Vector3.Distance(target.position + new Vector3(0, offset.y, 0), hit.point);
            
            if (hit.transform != target)
            {
                if (distanceToHit < distance)
                    targetDistance = distanceToHit - distanceOffsetFromCollisionHit;
            }
        }

        currentDistance = Mathf.MoveTowards(currentDistance, targetDistance, Time.deltaTime * distanceLerpRate);
        currentDistance = Mathf.Clamp(currentDistance, minDistance, distance);

        // Correct camera position
        finalPosition = target.position - (finalRotation * Vector3.forward * currentDistance);

        // Apply transform
        transform.rotation = finalRotation;
        transform.position = finalPosition;
        transform.position = AddLocalOffset(transform.position);

        // Rotation target
        if (IsRotateTarget)
        {
            Quaternion targetRotation = Quaternion.Euler(transform.rotation.x, transform.eulerAngles.y, transform.eulerAngles.z);
            target.rotation = Quaternion.RotateTowards(target.rotation, targetRotation, Time.deltaTime * rotateTargetLerpRate);
        }
    }

    private Vector3 AddLocalOffset(Vector3 position)
    {
        Vector3 result = position;
        result += new Vector3(0, offset.y, 0);
        result += transform.right * offset.x;
        result += transform.forward * offset.z;

        return result;
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }

    public void SetTargetOffset(Vector3 offset)
    {
        targetOffset = offset;
    }

    public void SetDefaultOffset()
    {
        targetOffset = defaultOffset;
    }
}