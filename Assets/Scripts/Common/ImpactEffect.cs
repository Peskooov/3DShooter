using UnityEngine;
using UnityEngine.Serialization;

public enum ImpactType
{
    NoDecal,
    Default
}

public class ImpactEffect : MonoBehaviour
{
    [SerializeField] private float lifeTime;
    [SerializeField] private GameObject decal;

    private float timer;

    private void Update()
    {
        if (timer < lifeTime)
            timer += Time.deltaTime;
        else
            Destroy(gameObject);
    }

    public void UpdateType(ImpactType type)
    {
        if (type == ImpactType.NoDecal)
        {
            decal.SetActive(false);
        }
    }
}