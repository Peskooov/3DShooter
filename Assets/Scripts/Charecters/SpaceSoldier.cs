using UnityEngine;

public class SpaceSoldier : Destructible
{
    protected override void OnDeath()
    {
        EventOnDeath?.Invoke();
    }
}
