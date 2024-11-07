using UnityEngine;

public enum WeaponMode
{
    Based,
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

    [SerializeField] private AudioClip launchSFX;
    public AudioClip LaunchSFX => launchSFX;
}