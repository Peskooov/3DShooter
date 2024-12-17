using System;
using UnityEngine;

public class ColliderViewPoints : MonoBehaviour
{
    private enum ColliderType
    {
        Character
    }

    [SerializeField] private ColliderType colliderType;
    [SerializeField] private Collider colliderObject;

    private Vector3[] points;

    private void Start()
    {
        if (colliderType == ColliderType.Character)
        {
            UpdatePointsForCharacterController();
        }
    }

    private void Update()
    {
        if (colliderType == ColliderType.Character)
        {
            CalculatePointsForCharacterController(colliderObject as CharacterController);
        }
    }

    public bool IsVisibleFromPoint(Vector3 point, Vector3 eyeDir, float viewAngle, float viewDistance)
    {
        for (int i = 0; i < points.Length; i++)
        {
            float angle = Vector3.Angle(points[i] - point, eyeDir);
            float dist = Vector3.Distance(points[i], point);

            if (angle <= viewAngle * 0.5f && dist <= viewDistance)
            {
                RaycastHit hit;

                Debug.DrawLine(point, points[i], Color.blue);
                if (Physics.Raycast(point, (points[i] - point).normalized, out hit, viewDistance * 2))
                {
                    if (hit.collider == colliderObject)
                        return true;
                }
            }
        }

        return false;
    }

    [ContextMenu("UpdateViewPoints")]
    private void UpdateViewPoints()
    {
        if (colliderObject == null) return;

        points = null;

        if (colliderType == ColliderType.Character)
        {
            UpdatePointsForCharacterController();
        }
    }

    private void UpdatePointsForCharacterController()
    {
        if (points == null)
        {
            points = new Vector3[4];
        }

        CharacterController characterCollider = colliderObject as CharacterController;

        CalculatePointsForCharacterController(characterCollider);
    }

    private void CalculatePointsForCharacterController(CharacterController collider)
    {
        points[0] = collider.transform.position + collider.center + collider.transform.up * collider.height * 0.3f;
        points[1] = collider.transform.position + collider.center - collider.transform.up * collider.height * 0.3f;
        points[2] = collider.transform.position + collider.center + collider.transform.right * collider.radius * 0.4f;
        points[3] = collider.transform.position + collider.center - collider.transform.right * collider.radius * 0.4f;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (points == null) return;

        Gizmos.color = Color.blue;
        for (int i = 0; i < points.Length; i++)
        {
            Gizmos.DrawSphere(points[i], 0.1f);
        }
    }
#endif
}