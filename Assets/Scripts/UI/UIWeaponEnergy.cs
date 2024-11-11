using UnityEngine;
using UnityEngine.UI;

public class UIWeaponEnergy : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    [SerializeField] private Slider slider;
    [SerializeField] private Image[] images;

    private void Start()
    {
        slider.maxValue = weapon.MaxPrimaryEnergy;
        slider.value = slider.maxValue;
    }

    private void Update()
    {
        slider.value = weapon.PrimaryEnergy;

        SetActiveImages(weapon.PrimaryEnergy != weapon.MaxPrimaryEnergy);
    }

    private void SetActiveImages(bool active)
    {
        for (int i = 0; i < images.Length; i++)
        {
            images[i].enabled = active;
        }
    }
}
