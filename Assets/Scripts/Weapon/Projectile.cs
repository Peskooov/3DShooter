using UnityEngine;

public class Projectile : ProjectileBase
{ /*
    [SerializeField] private ImpactEffect m_ImpactEffectPrefab;

    protected override void OnHit(Destructible destructible)
    {
        if (parent == Player.Instance.Ship)
        {
            if (destructible.HitPoints <= 0)
            {
                Player.Instance.AddScore(destructible.ScoreValue);

                if (destructible is SpaceShip)
                {
                    Player.Instance.AddKill();
                }
            }
        }
    }

    protected override void OnProjectilleLiveEnd(Collider2D collider2D, Vector2 position)
    {
        if (m_ImpactEffectPrefab != null)
            Instantiate(m_ImpactEffectPrefab, position, Quaternion.identity);

        Destroy(gameObject, 0);
    }*/
}