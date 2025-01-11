using UnityEngine;

public class AlienSoldier : Destructible, ISoundListener
{
    [SerializeField] private Weapon weapon;
    [SerializeField] private SpreadShootRig spreadShootRig;
    [SerializeField] private AIAlienSoldier aiAlienSoldier;
    [SerializeField] private float hearingDistance;

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

    // Serialize
    [System.Serializable]
    public class AIAlienState
    {
        public Vector3 position;
        public int hitPoins;
        public int behavior;

        public AIAlienState()
        {
        }
    }

    public override string SerializeState()
    {
        AIAlienState s = new AIAlienState();

        s.position = transform.position;
        s.hitPoins = HitPoints;
        s.behavior = (int)aiAlienSoldier.Behaviour;

        return JsonUtility.ToJson(s);
    }

    public override void DeserializeState(string state)
    {
        AIAlienState s = JsonUtility.FromJson<AIAlienState>(state);

        aiAlienSoldier.SetPosition(s.position);
        SetHitPoint(s.hitPoins);
        aiAlienSoldier.Behaviour = (AIAlienSoldier.AIBehaviour)s.behavior;
    }

    public void Heard(float distance)
    {
        if (distance <= hearingDistance)
            aiAlienSoldier.OnHeard();
    }
}