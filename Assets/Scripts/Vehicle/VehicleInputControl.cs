using UnityEngine;

public class VehicleInputControl : MonoBehaviour
{
    [SerializeField] private Vehicle vehicle;
    [SerializeField] private ThirdPersonCamera targetCamera;
    [SerializeField] private Vector3 cameraOffset;

    protected virtual void Start()
    {
        if (targetCamera != null)
        {
            targetCamera.IsRotateTarget = false;
            targetCamera.SetTargetOffset(cameraOffset);
        }
    }

    protected virtual void Update()
    {
        vehicle.SetTargetControl(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Jump"),
            Input.GetAxis("Vertical")));
        targetCamera.RotationControl = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }

    public void AssignCamera(ThirdPersonCamera thirdPersonCamera)
    {
        targetCamera = thirdPersonCamera;
        targetCamera.IsRotateTarget = false;
        targetCamera.SetTargetOffset(cameraOffset);
        targetCamera.SetTarget(vehicle.transform);
    }
}