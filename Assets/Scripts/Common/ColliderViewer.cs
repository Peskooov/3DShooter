using System;
using UnityEngine;

public class ColliderViewer : MonoBehaviour
{
    [SerializeField] private float viewingAngle;
    [SerializeField] private float viewingDistance;
    [SerializeField] private float viewHeight;
    [SerializeField] private float sideViewingAngle;
    [SerializeField] private float timeToDetection;

    public bool IsObjectVisible(GameObject target)
    {
        ColliderViewPoints viewPoints = target.GetComponent<ColliderViewPoints>();

        if (viewPoints == false) return false;

        return viewPoints.IsVisibleFromPoint(transform.position + new Vector3(0, viewHeight, 0), transform.forward,
            viewingAngle, viewingDistance);
    }

    public bool IsObjectVisibleFromSide(GameObject target)
    {
        ColliderViewPoints viewPoints = target.GetComponent<ColliderViewPoints>();

        if (viewPoints == false) return false;

        float timer = Time.time; // Запоминаем текущее время

        bool isVisible = viewPoints.IsVisibleFromPoint(transform.position + new Vector3(0, viewHeight, 0),
                             transform.right,
                             sideViewingAngle, viewingDistance) ||
                         viewPoints.IsVisibleFromPoint(transform.position + new Vector3(0, viewHeight, 0),
                             -transform.right,
                             sideViewingAngle, viewingDistance);

        if (Time.time - timer < timeToDetection)
        {
            return isVisible;
        }

        return false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.matrix = Matrix4x4.TRS(transform.position + new Vector3(0, viewHeight, 0), transform.rotation,
            Vector3.one);
        Gizmos.DrawFrustum(Vector3.zero, viewingAngle, 0, viewingDistance, 1);

        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(transform.position + new Vector3(0, viewHeight, 0), Quaternion.Euler(0, -90, 0),
            Vector3.one);
        Gizmos.DrawFrustum(transform.right, sideViewingAngle, 0, viewingDistance, 1);
        Gizmos.matrix = Matrix4x4.TRS(transform.position + new Vector3(0, viewHeight, 0), Quaternion.Euler(0, 90, 0),
            Vector3.one);
        Gizmos.DrawFrustum(transform.right, sideViewingAngle, 0, viewingDistance, 1);
    }
#endif
}