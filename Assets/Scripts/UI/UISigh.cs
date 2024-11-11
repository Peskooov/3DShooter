using UnityEngine;
using UnityEngine.UI;

public class UISigh : MonoBehaviour
{
    [SerializeField] private CharacterMovement characterMovement;
    [SerializeField] private Image sighImage;

    private void Update()
    {
        sighImage.enabled = characterMovement.IsAiming;
    }
}