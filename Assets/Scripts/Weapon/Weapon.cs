using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponMode mode;
    [SerializeField] private WeaponProperties weaponProperties;
    [SerializeField] private float maxPrimaryEnergy;
    [SerializeField] private Transform firePoint;
    
    public WeaponMode Mode => mode;
    public float MaxPrimaryEnergy => maxPrimaryEnergy;
    public float PrimaryEnergy => primaryEnergy;
    public bool CanFire => refireTimer <= 0 && energyIsRestored == false;
    
    private float refireTimer;
    private float primaryEnergy;
    private bool energyIsRestored;

    //private SpaceShip m_Ship;

    private void Start()
    {
        primaryEnergy = maxPrimaryEnergy;

        //m_Ship = transform.parent.GetComponent<SpaceShip>();
    }

    private void Update()
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
        if(energyIsRestored) return;
        if (!CanFire) return;
        if (weaponProperties == null) return;
        if (refireTimer > 0) return;
        if (TryDrawEnergy(weaponProperties.EnergyForUse) == false) return;

        Projectile projectile = Instantiate(weaponProperties.ProjectilePrefab).GetComponent<Projectile>();
        projectile.transform.position = firePoint.position;
        projectile.transform.forward = firePoint.forward;

        //projectile.SetParentShooter(m_Ship);

        refireTimer = weaponProperties.RateOfFire;

        {
            //SFX
        }
    }

    public void FirePointLookAt(Vector3 pos)
    {
        firePoint.LookAt(pos);
    }
    
    public void AssignLoadOut(WeaponProperties properties)
    {
        if (mode != properties.Mode) return;

        refireTimer = 0;
        weaponProperties = properties;
    }
    
    private bool TryDrawEnergy(int count)
    {
        if(count == 0)
        return true;

        if (primaryEnergy >= 0)
        {
            primaryEnergy -= count;
            return true;
        }

        energyIsRestored = true;
        
        return false;
    }
}