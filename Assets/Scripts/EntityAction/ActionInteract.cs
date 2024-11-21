using System;
using UnityEngine;

public enum InteractType
{
    PickupItem,
    EnteringCode,
    DisableDrone
}

[Serializable]
public class ActionInteractProperties : EntityActionProperties
{
    [SerializeField] private Transform interactTransform;
    public Transform InteractTransform => interactTransform;
}

public class ActionInteract : EntityContextAction
{
    [SerializeField] private InteractType type;
    [SerializeField] private Transform owner;
    
    public InteractType Type => type;
    
    private ActionInteractProperties properties;

    private bool isFreezePosition;
    public bool IsFreezePosition => isFreezePosition;
    
    public override void SetProperties(EntityActionProperties properties)
    {
        this.properties = properties as ActionInteractProperties;
    }

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
    }
}
