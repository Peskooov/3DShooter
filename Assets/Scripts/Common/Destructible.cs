using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Destructible : Entity, ISerializableEntity
{
    [SerializeField] private bool isIndestructable;

    public bool IsIndestructable => isIndestructable;
    [SerializeField] private int hitPoints;
    public int MaxHitPoints => hitPoints;
    private int currentHitPoints;
    public int HitPoints => currentHitPoints;

    private bool isDeath;
    public bool IsDeath => isDeath;

    protected virtual void Start()
    {
        currentHitPoints = hitPoints;
    }

    public void ApplyDamage(int damage, Destructible other)
    {
        if (isIndestructable || isDeath) return;

        currentHitPoints -= damage;

        OnGetDamage?.Invoke(other);
        eventOnGetDamage?.Invoke();

        if (currentHitPoints <= 0)
        {
            OnDeath();
            isDeath = true;
        }
    }

    public void ApplyHeal(int heal)
    {
        currentHitPoints += heal;

        if (currentHitPoints > hitPoints)
            currentHitPoints = hitPoints;
    }

    public void HealFull()
    {
        currentHitPoints = hitPoints;
    }

    public void BlockDamage(float blockTime)
    {
        currentHitPoints = hitPoints;

        isIndestructable = true;
    }

    public void GetDamage()
    {
        isIndestructable = false;
    }

    protected virtual void OnDeath()
    {
        Destroy(gameObject);

        EventOnDeath?.Invoke();
    }

    public static Destructible FindNearest(Vector3 position)
    {
        float minDist = float.MaxValue;
        Destructible target = null;

        foreach (Destructible dest in allDestructables)
        {
            float curDist = Vector3.Distance(dest.transform.position, position);

            if (curDist < minDist)
            {
                minDist = curDist;
                target = dest;
            }
        }

        return target;
    }

    public static Destructible FindNearestNonTeamMember(Destructible destructible)
    {
        float minDist = float.MaxValue;
        Destructible target = null;

        foreach (Destructible dest in allDestructables)
        {
            float curDist = Vector3.Distance(dest.transform.position, destructible.transform.position);

            if (curDist < minDist && destructible.TeamID != dest.TeamID)
            {
                minDist = curDist;
                target = dest;
            }
        }

        return target;
    }

    public static List<Destructible> GetAllTeamMember(int teamID)
    {
        List<Destructible> teamDestructible = new List<Destructible>();

        foreach (Destructible dest in allDestructables)
        {
            if (dest.TeamID == teamID)
                teamDestructible.Add(dest);
        }

        return teamDestructible;
    }

    public static List<Destructible> GetAllNonTeamMember(int teamID)
    {
        List<Destructible> teamDestructible = new List<Destructible>();

        foreach (Destructible dest in allDestructables)
        {
            if (dest.TeamID != teamID)
                teamDestructible.Add(dest);
        }

        return teamDestructible;
    }

    private static HashSet<Destructible> allDestructables;

    public static IReadOnlyCollection<Destructible> AllDestructable => allDestructables;

    protected virtual void OnEnable()
    {
        if (allDestructables == null)
        {
            allDestructables = new HashSet<Destructible>();
        }

        allDestructables.Add(this);
    }

    protected virtual void OnDestroy()
    {
        allDestructables.Remove(this);
    }

    public const int TeamIdNeuTral = 0;

    [SerializeField] private int m_TeamId;
    public int TeamID => m_TeamId;

    public UnityEvent EventOnDeath;

    [SerializeField] private UnityEvent eventOnGetDamage;
    public UnityAction<Destructible> OnGetDamage;

    [SerializeField] private int m_ScoreValue;
    public int ScoreValue => m_ScoreValue;

    // Serialize
    [System.Serializable]
    public class State
    {
        public Vector3 position;
        public int _hitPoins;

        public State()
        {
        }
    }

    [SerializeField] private int m_EntityId;
    public long EntityId => m_EntityId;

    public bool IsSerializable()
    {
        return currentHitPoints > 0;
    }

    public string SerializeState()
    {
        State s = new State();

        s.position = transform.position;
        s._hitPoins = currentHitPoints;

        return JsonUtility.ToJson(s);
    }

    public void DeserializeState(string state)
    {
        State s = JsonUtility.FromJson<State>(state);

        transform.position = s.position;
        hitPoints = s._hitPoins;
    }
}