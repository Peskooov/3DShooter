using UnityEngine;

public class AlienSoldier : Destructible
{
    protected override void OnDeath()
    {
        EventOnDeath?.Invoke();
    }
}