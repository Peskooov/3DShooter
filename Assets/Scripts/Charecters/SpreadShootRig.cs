using UnityEngine;
using UnityEngine.Animations.Rigging;

public class SpreadShootRig : MonoBehaviour
{
    [SerializeField] private CharacterMovement targetCharacter;
    [SerializeField] private Rig spreadRig;

    [SerializeField] private float changeWeightLerpRate;
    private float targetWeight;
    
    private void Update()
    {
        spreadRig.weight = Mathf.MoveTowards(spreadRig.weight, targetWeight, Time.deltaTime * changeWeightLerpRate);

        if (spreadRig.weight >= 1)
            targetWeight = 0;
    }

    public void Spread()
    {
        targetWeight = 1;
    }
}
