using System;
using UnityEngine;

public enum InteractType
{
    PickupItem
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
    
    private new ActionInteractProperties properties;
    
    public override void SetProperties(EntityActionProperties properties)
    {
        this.properties = properties as ActionInteractProperties;
    }

    public override void StartAction()
    {
        if(!IsCanStart) return;
        
        base.StartAction();

        owner.position = properties.InteractTransform.position;
    }
}
