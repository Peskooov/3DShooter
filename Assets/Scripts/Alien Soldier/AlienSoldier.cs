using UnityEngine;

public class AlienSoldier : Destructible
{
    [SerializeField] private Weapon weapon;
    [SerializeField] private SpreadShootRig spreadShootRig;


    protected override void OnDeath()
    {
        EventOnDeath?.Invoke();
    }

    public void Fire(Vector3 target)
    {
        if (weapon.CanFire == false) return;

        weapon.FirePointLookAt(target);
        weapon.Fire();

        spreadShootRig.Spread();
    }
}