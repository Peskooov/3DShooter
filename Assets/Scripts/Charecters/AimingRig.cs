using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AimingRig : MonoBehaviour
{
    [SerializeField] private CharacterMovement targetCharacter;
    [SerializeField] private Rig[] rigs;

    [SerializeField] private float changeWeightLerpRate;
    private float targetWeight;
    
    private void Update()
    {
        for (int i = 0; i < rigs.Length; i++)
        {
            rigs[i].weight = Mathf.MoveTowards(rigs[i].weight, targetWeight, Time.deltaTime * changeWeightLerpRate);
        }

        if (targetCharacter.IsAiming)
            targetWeight = 1;
        else
            targetWeight = 0;
    }
}
