using UnityEngine;

public class SpaceSoldier : Destructible
{
    [SerializeField] private CharacterMovement characterMovement;
    [SerializeField] private float fallDamageFactor;

    protected override void Start()
    {
        base.Start();
        characterMovement.Land += OnLand;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        characterMovement.Land -= OnLand;
    }

    protected override void OnDeath()
    {
        characterMovement.Land -= OnLand;

        EventOnDeath?.Invoke();
    }

    private void OnLand(Vector3 vel)
    {
        if (Mathf.Abs(vel.y) <= 10) return;

        ApplyDamage((int)Mathf.Abs(vel.y) * (int)fallDamageFactor, this);
    }
}