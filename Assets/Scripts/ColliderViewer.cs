using System;
using UnityEngine;

public class ColliderViewer : MonoBehaviour
{
    [SerializeField] private float viewingAngle;
    [SerializeField] private float viewingDistance;
    [SerializeField] private float viewHeight;


    public bool IsObjectVisible(GameObject target)
    {
        ColliderViewPoints viewPoints = target.GetComponent<ColliderViewPoints>();

        if (viewPoints == false) return false;

        return viewPoints.IsVisibleFromPoint(transform.position + new Vector3(0, viewHeight, 0), transform.forward,
            viewingAngle, viewingDistance);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.matrix = Matrix4x4.TRS(transform.position + new Vector3(0, viewHeight, 0), transform.rotation,
            Vector3.one);
        Gizmos.DrawFrustum(Vector3.zero, viewingAngle, 0, viewingDistance, 1);
    }
#endif
}