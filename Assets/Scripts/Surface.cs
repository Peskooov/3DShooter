using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Surface : MonoBehaviour
{
    [SerializeField] private ImpactType impactType;
    public ImpactType Type => impactType;

    [ContextMenu("AddToAllObjects")]
    public void AddToAllObjects()
    {
        Transform[] transforms = GameObject.FindObjectsOfType<Transform>();

        for (int i = 0; i < transforms.Length; i++)
        {
            if (transforms[i].GetComponent<Collider>() != null)
            {
                if (transforms[i].GetComponent<Surface>() == null)
                {
                    transforms[i].gameObject.AddComponent<Surface>();
                }
            }
        }
    }
}