using UnityEngine;

public class Turret : Weapon
{
    [SerializeField] private Transform basedTransform;

    [SerializeField] private Transform gun;

    [SerializeField] private Transform aim;

    [SerializeField] private float m_RotationLerpFactor;

    protected Quaternion BaseTargetRotation;
    protected Quaternion BaseRotation;
    protected Quaternion GunTargetRotation;
    protected Vector3 GunRotation;

    protected override void Update()
    {
        base.Update();

        LookOnAim();
    }

    private void LookOnAim()
    {
        BaseTargetRotation =
            Quaternion.LookRotation(new Vector3(aim.position.x, gun.position.y, aim.position.z) - gun.position);
        BaseRotation = Quaternion.RotateTowards(basedTransform.localRotation, BaseTargetRotation,
            Time.deltaTime * m_RotationLerpFactor);
        basedTransform.localRotation = BaseRotation;

        GunTargetRotation = Quaternion.LookRotation(aim.position - basedTransform.position);
        GunRotation = Quaternion.RotateTowards(gun.rotation, GunTargetRotation, Time.deltaTime * m_RotationLerpFactor)
            .eulerAngles;
        gun.rotation = BaseRotation * Quaternion.Euler(GunRotation.x, 0, 0);
    }

    public void SetAim(Transform aim)
    {
        this.aim = aim;
    }
}