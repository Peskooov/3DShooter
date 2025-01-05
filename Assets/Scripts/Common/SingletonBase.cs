using UnityEngine;

[DisallowMultipleComponent]
public abstract class SingletonBase<T> : MonoBehaviour where T : MonoBehaviour
{
    [Header("Singleton")] [SerializeField] private bool doNotDestroyOnLoad;

    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("MonoSingleton: object of type already on scene" + typeof(T).Name);
            Destroy(this);
            return;
        }

        Instance = this as T;

        if (doNotDestroyOnLoad)
            DontDestroyOnLoad(gameObject);
    }

    public void Destroy()
    {
        Instance = null;
    }
}