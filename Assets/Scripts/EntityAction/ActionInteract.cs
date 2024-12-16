using System;
using UnityEngine;

public enum InteractType
{
    PickupItem,
    EnteringCode,
    DisableDrone,
    UseVehicle
}

[Serializable]
public class ActionInteractProperties : EntityActionProperties
{
    [SerializeField] private Transform interactTransform;
    public Transform InteractTransform => interactTransform;
}

public class ActionInteract : EntityContextAction
{
    [SerializeField] protected Transform owner;
    [SerializeField] private InteractType type;

    public InteractType Type => type;

    protected new ActionInteractProperties Properties;

    private bool isFreezePosition;
    public bool IsFreezePosition => isFreezePosition;

    public override void SetProperties(EntityActionProperties prop)
    {
        Properties = prop as ActionInteractProperties;
    }

    /*
    private void Update()
    {
        if (isFreezePosition)
        {
            owner.position = Vector3.MoveTowards(owner.position, properties.InteractTransform.position, Time.deltaTime);
            owner.rotation = Quaternion.LookRotation( properties.InteractTransform.forward, Vector3.up);
        }
    }

    public override void StartAction()
    {
        if(!IsCanStart) return;

        base.StartAction();
        isFreezePosition = true;
    }

    public override void EndAction()
    {
        base.EndAction();
        isFreezePosition = false;
    }*/
}