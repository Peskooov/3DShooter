using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponMode mode;
    public WeaponMode Mode => mode;

    [SerializeField] private WeaponProperties weaponProperties;

    private float refireTimer;

    public bool CanFire => refireTimer <= 0;

    //private SpaceShip m_Ship;

    private void Start()
    {
        //m_Ship = transform.parent.GetComponent<SpaceShip>();
    }

    private void Update()
    {
        if (refireTimer > 0)
            refireTimer -= Time.deltaTime;
    }

    public void Fire()
    {
        if (weaponProperties == null) return;
        if (refireTimer > 0) return;

        //if (m_Ship.DrawEnergy(weaponProperties.EnergyForUse) == false)
        //    return;

        Projectile projectile = Instantiate(weaponProperties.ProjectilePrefab).GetComponent<Projectile>();
        projectile.transform.position = transform.position;
        projectile.transform.up = transform.up;

        //projectile.SetParentShooter(m_Ship);

        refireTimer = weaponProperties.RateOfFire;

        {
            //SFX
        }
    }

    public void AssignLoadOut(WeaponProperties properties)
    {
        if (mode != properties.Mode) return;

        refireTimer = 0;
        weaponProperties = properties;
    }
}