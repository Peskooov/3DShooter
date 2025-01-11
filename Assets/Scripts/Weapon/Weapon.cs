using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponMode mode;
    [SerializeField] private WeaponProperties weaponProperties;
    [SerializeField] private float maxPrimaryEnergy;
    [SerializeField] private Transform firePoint;

    [SerializeField] private ParticleSystem muzzleParticleSystem;
    [SerializeField] private NoiseAudioSource audioSource;

    public WeaponMode Mode => mode;
    public float MaxPrimaryEnergy => maxPrimaryEnergy;
    public float PrimaryEnergy => primaryEnergy;
    public bool CanFire => refireTimer <= 0 && energyIsRestored == false;

    private float refireTimer;
    private float primaryEnergy;
    private bool energyIsRestored;

    private Destructible owner;

    private void Start()
    {
        primaryEnergy = maxPrimaryEnergy;

        owner = transform.root.GetComponent<Destructible>();
    }

    protected virtual void Update()
    {
        if (refireTimer > 0)
            refireTimer -= Time.deltaTime;

        UpdateEnergy();
    }

    private void UpdateEnergy()
    {
        primaryEnergy += (float)weaponProperties.EnergyRegenPerSecond * Time.deltaTime;
        primaryEnergy = Mathf.Clamp(primaryEnergy, 0, maxPrimaryEnergy);

        if (primaryEnergy >= weaponProperties.EnergyAmountToStartFire)
            energyIsRestored = false;
    }

    public void Fire()
    {
        if (energyIsRestored) return;
        if (!CanFire) return;
        if (!weaponProperties) return;
        if (refireTimer > 0) return;
        if (TryDrawEnergy(weaponProperties.EnergyForUse) == false) return;

        Projectile projectile = Instantiate(weaponProperties.ProjectilePrefab).GetComponent<Projectile>();
        projectile.transform.position = firePoint.position;
        projectile.transform.forward = firePoint.forward;

        projectile.SetParentShooter(owner);

        refireTimer = weaponProperties.RateOfFire;

        {
            if (muzzleParticleSystem)
            {
                muzzleParticleSystem.time = 0;
                muzzleParticleSystem.Play();
            }

            if (audioSource)
            {
                audioSource.clip = weaponProperties.LaunchSFX;
                audioSource.Play();
            }
        }
    }

    public void FirePointLookAt(Vector3 pos)
    {
        Vector3 offset = Random.insideUnitSphere * weaponProperties.SpreadShootRange;

        if (weaponProperties.SpreadShootDistanceFactor != 0)
        {
            offset = offset * Vector3.Distance(firePoint.position, pos) * weaponProperties.SpreadShootDistanceFactor;
        }

        firePoint.LookAt(pos + offset);
    }

    public void AssignLoadOut(WeaponProperties properties)
    {
        if (mode != properties.Mode) return;

        refireTimer = 0;
        weaponProperties = properties;
    }

    private bool TryDrawEnergy(int count)
    {
        if (count == 0) return true;

        if (primaryEnergy >= count)
        {
            primaryEnergy -= count;
            return true;
        }

        energyIsRestored = true;

        return false;
    }
}