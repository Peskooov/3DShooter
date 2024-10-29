using UnityEngine;
using UnityEngine.UIElements;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float distance;
    [SerializeField] private Vector3 offset;

    [Header("Rotation Limit")] 
    [SerializeField] private float minLimitY;
    [SerializeField] private float maxLimitY;
    
    private float deltaRotationX;
    private float deltaRotationY;

    private void Update()
    {
        deltaRotationX += Input.GetAxis("Mouse X");
        deltaRotationY += Input.GetAxis("Mouse Y");

        deltaRotationY = ClampAngle(deltaRotationY, minLimitY, maxLimitY);
        
        Quaternion finalRotation = Quaternion.Euler(-deltaRotationY,deltaRotationX,0);
        Vector3 finalPosition = target.position - (finalRotation * Vector3.forward * distance);

        finalPosition = AddLocalOffset(finalPosition);
        
        transform.rotation = finalRotation;
        transform.position = finalPosition;

        if (Input.GetMouseButton(1))
        {
            target.rotation= Quaternion.Euler(transform.rotation.x,transform.eulerAngles.y, transform.eulerAngles.z);
        }
    }

    private Vector3 AddLocalOffset(Vector3 position)
    {
        Vector3 result = position;
        result.y = offset.y;
        
        return position;
    }
    
    private float ClampAngle(float angle, float min, float max)
    {
        return angle;
    }
}
