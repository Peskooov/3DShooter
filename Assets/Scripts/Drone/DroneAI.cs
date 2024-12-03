using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Drone))]
public class DroneAI : MonoBehaviour
{
    [SerializeField] private CubeArea movementArea;
    [SerializeField] private ColliderViewer colliderViewer;

    private Drone drone;
    private Vector3 movementPosition;
    private Transform shootTarget;

    private void Start()
    {
        drone = GetComponent<Drone>();
        drone.EventOnDeath.AddListener(OnDroneDeath);

        drone.OnGetDamage += OnGetDamage;
    }

    private void OnDestroy()
    {
        drone.EventOnDeath.RemoveListener(OnDroneDeath);

        drone.OnGetDamage -= OnGetDamage;
    }

    private void Update()
    {
        UpdateAI();
    }

    private void OnGetDamage(Destructible other)
    {
        ActionAssignTargetAllTeamMember(other.transform);
    }

    private void OnDroneDeath()
    {
        enabled = false;
    }

    private void UpdateAI()
    {
        if (drone.IsDisabled) return;

        ActionFindShootingTarget();
        ActionMove();
        ActionFire();
    }

    //Actions
    private void ActionFindShootingTarget()
    {
        Transform potentialTarget = FindShootTarget();

        if (potentialTarget != null)
            ActionAssignTargetAllTeamMember(potentialTarget);
    }

    private void ActionMove()
    {
        if (transform.position == movementPosition)
            movementPosition = movementArea.GetRandomInsideZone();

        if (Physics.Linecast(transform.position, movementPosition))
            movementPosition = movementArea.GetRandomInsideZone();

        drone.MoveTo(movementPosition);

        if (shootTarget)
            drone.LookAt(shootTarget.position);
        else
            drone.LookAt(movementPosition);
    }

    private void ActionFire()
    {
        if (shootTarget != null)
        { // добавить если виден
            drone.Fire(shootTarget.position);
        }
    }

    //Methods
    private void SetShootTarget(Transform target)
    {
        shootTarget = target;
    }

    private Transform FindShootTarget()
    {
        List<Destructible> targets = Destructible.GetAllNonTeamMember(drone.TeamID);

        for (int i = 0; i < targets.Count; i++)
        {
            Debug.Log(colliderViewer.IsObjectVisible(targets[i].gameObject));

            if (colliderViewer.IsObjectVisible(targets[i].gameObject))
                return targets[i].transform;
        }

        return null;
    }

    private void ActionAssignTargetAllTeamMember(Transform other)
    {
        List<Destructible> team = Destructible.GetAllTeamMember(drone.TeamID);

        foreach (Destructible dest in team)
        {
            DroneAI ai = dest.transform.root.GetComponent<DroneAI>();

            if (ai != null && ai.enabled)
            {
                ai.SetShootTarget(other);
            }
        }
    }
}