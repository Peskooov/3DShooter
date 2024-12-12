using System;
using UnityEngine;

[System.Serializable]
public class WheelAxle
{
    [SerializeField] private WheelCollider leftWheelCollider;
    [SerializeField] private WheelCollider rightWheelCollider;

    [SerializeField] private Transform leftWheelMesh;
    [SerializeField] private Transform rightWheelMesh;

    [SerializeField] private bool motor;
    public bool Motor => motor;

    [SerializeField] private bool steering;
    public bool Steering => steering;

    public void SetTorque(float torque)
    {
        if (!motor) return;

        leftWheelCollider.motorTorque = torque;
        rightWheelCollider.motorTorque = torque;
    }

    public void Break(float breakTorque)
    {
        leftWheelCollider.brakeTorque = breakTorque;
        rightWheelCollider.brakeTorque = breakTorque;
    }

    public void SetSteerAngle(float angle)
    {
        if (!steering) return;

        leftWheelCollider.steerAngle = angle;
        rightWheelCollider.steerAngle = angle;
    }

    public void UpdateMeshTransform()
    {
        UpdateWheelTransform(leftWheelCollider, ref leftWheelMesh);
        UpdateWheelTransform(rightWheelCollider, ref rightWheelMesh);
    }

    private void UpdateWheelTransform(WheelCollider wheelCollider, ref Transform wheelTransform)
    {
        Vector3 position;
        Quaternion rotation;

        wheelCollider.GetWorldPose(out position, out rotation);
        wheelTransform.position = position;
        wheelTransform.rotation = rotation;
    }
}

[RequireComponent(typeof(Rigidbody))]
public class WheeledVehicle : Vehicle
{
    [SerializeField] private WheelAxle[] wheelAxles;
    [SerializeField] private float maxMotorTorque;
    [SerializeField] private float breakTorque;
    [SerializeField] private float maxSteerAngle;

    private Rigidbody rb;

    public override float LinearVelocity => rb.velocity.magnitude;

    private void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float targetMotor = maxMotorTorque * TargetInputControl.z;
        float brakeTorque = breakTorque * TargetInputControl.y;
        float steering = maxSteerAngle * TargetInputControl.x;

        for (int i = 0; i < wheelAxles.Length; i++)
        {
            if (brakeTorque == 0 && LinearVelocity < maxLinearVelocity)
            {
                wheelAxles[i].Break(0);
                wheelAxles[i].SetTorque(targetMotor);
            }

            if (LinearVelocity > maxLinearVelocity)
                wheelAxles[i].Break(brakeTorque * 0.2f);
            else
                wheelAxles[i].Break(brakeTorque);

            wheelAxles[i].SetSteerAngle(steering);
            wheelAxles[i].UpdateMeshTransform();
        }
    }
}