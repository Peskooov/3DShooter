using UnityEngine;

public class UseVehicleTrigger : TriggerInteractAction
{
    [SerializeField] private ActionUseVehicleProperties useProperties;

    protected override void InitActionProperties()
    {
        action.SetProperties(useProperties);
    }
}