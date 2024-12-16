using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HoverVehicle : Vehicle
{
    [SerializeField] private float thrustForward;
    [SerializeField] private float thrustTorque;
    [SerializeField] private float dragLinear;
    [SerializeField] private float dragAngular;
    [SerializeField] private float hoverHeight;
    [SerializeField] private float hoverForce;
    [SerializeField] private float maxLinearSpeed;
    [SerializeField] private Transform[] hoverJets;

    private Rigidbody rb;

    private bool isGrounded;

    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        ComputeForces();
    }

    private void ComputeForces()
    {
        isGrounded = false;

        for (int i = 0; i < hoverJets.Length; i++)
        {
            if (ApplyJetForce(hoverJets[i]))
            {
                isGrounded = true;
            }
        }

        if (isGrounded)
        {
            rb.AddRelativeForce(Vector3.forward * thrustForward * TargetInputControl.z);
            rb.AddRelativeTorque(Vector3.up * thrustTorque * TargetInputControl.x);
        }

        //Linear drag
        float dragCoeff = thrustForward / maxLinearVelocity;
        Vector3 dragForce = rb.velocity * -dragCoeff;

        if (isGrounded)
        {
            rb.AddForce(dragForce, ForceMode.Acceleration);
        }

        //Angular drag
        Vector3 angularForce = -rb.angularVelocity * dragAngular;
        rb.AddTorque(angularForce, ForceMode.Acceleration);
    }

    public bool ApplyJetForce(Transform tr)
    {
        Ray ray = new Ray(tr.position, -tr.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, hoverHeight))
        {
            float s = (hoverHeight - hit.distance) / hoverHeight;
            Vector3 force = (s * hoverForce) * hit.normal;

            rb.AddForceAtPosition(force, tr.position, ForceMode.Acceleration);

            return true;
        }

        return false;
    }
}