using System;
using UnityEngine;

[Serializable]
public class ActionUseVehicleProperties : ActionInteractProperties
{
    public Vehicle Vehicle;
    public VehicleInputControl VehicleInputControl;
    public GameObject Hint;
    public Transform[] ExitPointsTransform;
}

public class ActionUseVehicle : ActionInteract
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private CharacterMovement characterMovement;
    [SerializeField] private CharacterInputController characterInputController;
    [SerializeField] private GameObject visualModel;
    [SerializeField] private ThirdPersonCamera thirdPersonCamera;

    private bool inVehicle;

    private void Start()
    {
        EventOnStart.AddListener(OnActionStarted);
        EventOnEnd.AddListener(OnActionEnded);
    }

    private void OnDestroy()
    {
        EventOnStart.RemoveListener(OnActionStarted);
        EventOnEnd.RemoveListener(OnActionEnded);
    }

    private void Update()
    {
        if (inVehicle == true)
        {
            IsCanEnd = (Properties as ActionUseVehicleProperties).Vehicle.LinearVelocity < 2;
            (Properties as ActionUseVehicleProperties).Hint.SetActive(IsCanEnd);
        }
    }

    private void OnActionStarted()
    {
        ActionUseVehicleProperties prop = Properties as ActionUseVehicleProperties;

        if (prop == null || prop.Vehicle == null || prop.VehicleInputControl == null)
        {
            Debug.LogError("ActionUseVehicleProperties or its properties are not set.");
            return; // Exit if any required property is null
        }

        inVehicle = true;

        //Camera
        prop.VehicleInputControl.AssignCamera(thirdPersonCamera);

        //VehicleInput
        prop.VehicleInputControl.enabled = true;
        prop.Vehicle.enabled = true; //включаем возможность перемещения транспорта
        prop.Vehicle.GetComponent<Rigidbody>().isKinematic = false; //возвращаем транспорту влияние физики.

        //CharacterInput
        characterInputController.enabled = false;

        //CharacterMovement
        characterController.enabled = false;
        characterMovement.enabled = false;

        //Hide visual model
        visualModel.transform.localPosition = visualModel.transform.localPosition + new Vector3(0, 10000, 0);
    }

    private void OnActionEnded()
    {
        ActionUseVehicleProperties prop = Properties as ActionUseVehicleProperties;

        inVehicle = false;

        //Camera
        characterInputController.AssignCamera(thirdPersonCamera);

        //VehicleInput
        prop.VehicleInputControl.enabled = false;
        prop.Vehicle.enabled = false;
        prop.Vehicle.GetComponent<Rigidbody>().isKinematic = true;

        //CharacterInput
        characterInputController.enabled = true;

        //CharacterMovement
        //owner.position = prop.InteractTransform.position;
        ExitObstacles();
        characterController.enabled = true;
        characterMovement.enabled = true;

        //Show visual model
        visualModel.transform.localPosition = new Vector3(0, 0, 0);
    }

    private void ExitObstacles()
    {
        ActionUseVehicleProperties prop = Properties as ActionUseVehicleProperties;

        for (int i = 0; i < prop.ExitPointsTransform.Length; i++)
        {
            Collider[] hitColliders = Physics.OverlapSphere(prop.ExitPointsTransform[i].position, 0.1f);

            if (hitColliders.Length == 0)
            {
                owner.position = prop.ExitPointsTransform[i].position;
                break;
            }
        }
    }
}