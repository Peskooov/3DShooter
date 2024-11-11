using UnityEngine;

public enum WeaponMode
{
    Primary,
    Secondary
}

[CreateAssetMenu]
public sealed class WeaponProperties : ScriptableObject
{
    [SerializeField] private WeaponMode mode;
    public WeaponMode Mode => mode;

    [SerializeField] private Projectile projectilePrefab;
    public Projectile ProjectilePrefab => projectilePrefab;

    [SerializeField] private float rateOfFite;
    public float RateOfFire => rateOfFite;

    [SerializeField] private int energyForUse;
    public int EnergyForUse => energyForUse;

    [SerializeField] private int energyRegenPerSecond;
    public int EnergyRegenPerSecond => energyRegenPerSecond;

    [SerializeField] private int energyAmountToStartFire;
    public int EnergyAmountToStartFire => energyAmountToStartFire;

    [SerializeField] private AudioClip launchSFX;
    public AudioClip LaunchSFX => launchSFX;
}