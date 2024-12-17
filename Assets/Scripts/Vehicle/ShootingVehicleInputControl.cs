using UnityEngine;

public class ShootingVehicleInputControl : VehicleInputControl
{
    [SerializeField] private CameraShooter cameraShooter;
    [SerializeField] private Transform aimPoint;

    protected override void Update()
    {
        base.Update();

        aimPoint.position = cameraShooter.Camera.transform.position + cameraShooter.Camera.transform.forward * 30;

        if (Input.GetMouseButton(0))
        {
            cameraShooter.Shoot();
        }
    }
}