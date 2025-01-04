using System;
using UnityEngine;

public class LightningSwitch : MonoBehaviour
{
    [SerializeField] private VehicleInputControl vehicleInputControl;
    [SerializeField] private GameObject light;

    private void Update()
    {
        light.SetActive(vehicleInputControl.enabled);
    }
}