using UnityEngine;
using UnityEngine.Events;

public abstract class EntityActionProperties
{
}

public abstract class EntityAction : MonoBehaviour
{
    [SerializeField] private UnityEvent eventOnStart;
    [SerializeField] private UnityEvent eventOnEnd;

    public UnityEvent EventOnStart => eventOnStart;
    public UnityEvent EventOnEnd => eventOnEnd;

    private EntityActionProperties properties;
    public EntityActionProperties Properties => properties;

    private bool isStarted;

    public virtual void StartAction()
    {
        if (isStarted == true) return;

        isStarted = true;
        eventOnStart?.Invoke();
    }

    public virtual void EndAction()
    {
        isStarted = false;
        eventOnEnd?.Invoke();
    }

    public virtual void SetProperties(EntityActionProperties prop)
    {
        properties = prop;
    }
}