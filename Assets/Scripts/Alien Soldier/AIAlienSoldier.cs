using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class AIAlienSoldier : MonoBehaviour
{
    public enum AIBehaviour
    {
        Null = 0,
        Idle = 1,
        PatrolRandom = 2,
        PatrolCircle = 3,
        PursueTarget = 4,
        SeekTarget = 5,
        FindNearestPoint = 6
    }

    [SerializeField] private AIBehaviour aIBehaviour;
    [SerializeField] private AlienSoldier alienSoldier;
    [SerializeField] private CharacterMovement characterMovement;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private PatrolPath patrolPath;
    [SerializeField] private ColliderViewer colliderViewer;
    [SerializeField] private float aimingDistance;
    [SerializeField] private float findRange;
    [SerializeField] private int patrolPathNodeIndex = 0;

    public event UnityAction<bool> OnVisibilityChanged;

    private NavMeshPath navMeshPath;
    private PatrolPathNode currentPathNode;

    private GameObject potentialTarget;
    private Transform pursueTarget;
    private Vector3 seekTarget;
    private Vector3 findRandomTarget;

    private void Start()
    {
        potentialTarget = Destructible.FindNearestNonTeamMember(alienSoldier)?.gameObject;

        characterMovement.UpdatePosition = false;
        navMeshPath = new NavMeshPath();

        StartBehaviour(aIBehaviour);

        alienSoldier.OnGetDamage += OnGetDamage;
    }

    private void OnDestroy()
    {
        alienSoldier.OnGetDamage -= OnGetDamage;
    }

    private void Update()
    {
        SyncAgentAndCharacterMovement();
        UpdateAI();
    }

    private void OnGetDamage(Destructible other)
    {
        if (other.TeamID != alienSoldier.TeamID)
        {
            ActionAssignTargetAllTeamMember(other.transform);
        }
    }

    private void CheckPlaySound()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 15f);

        foreach (var collider in colliders)
        {
            AudioSource audioSource = collider.GetComponentInChildren<AudioSource>();

            if (audioSource != null && audioSource != alienSoldier.GetComponentInChildren<AudioSource>() &&
                audioSource.isPlaying)
            {
                ActionAssignTargetAllTeamMember(audioSource.transform);
            }
        }
    }

    private void UpdateAI()
    {
        ActionUpdateTarget(); // potentialTarget = Destructible.FindNearestNonTeamMember(alienSoldier)?.gameObject; можно вызывать в методе например с таймером
        CheckPlaySound();

        if (aIBehaviour == AIBehaviour.Idle) return;

        if (aIBehaviour == AIBehaviour.PursueTarget)
        {
            agent.CalculatePath(pursueTarget.position, navMeshPath);
            agent.SetPath(navMeshPath);

            if (Vector3.Distance(transform.position, pursueTarget.position) <= aimingDistance)
            {
                characterMovement.Aiming();
                alienSoldier.Fire(pursueTarget.position + new Vector3(0, 1, 0));
            }
            else
            {
                characterMovement.UnAiming();
            }
        }

        if (aIBehaviour == AIBehaviour.SeekTarget)
        {
            agent.CalculatePath(seekTarget, navMeshPath);
            agent.SetPath(navMeshPath);

            if (AgentReachedDestination())
            {
                StartBehaviour(AIBehaviour.PatrolRandom);
            }
        }

        if (aIBehaviour == AIBehaviour.FindNearestPoint)
        {
            Vector3 newPosition = GenerateRandomPositionAround(transform.position);
            findRandomTarget = newPosition;

            agent.CalculatePath(findRandomTarget, navMeshPath);
            agent.SetPath(navMeshPath);

            if (AgentReachedDestination())
            {
                StartBehaviour(AIBehaviour.SeekTarget);
            }
        }

        if (aIBehaviour == AIBehaviour.PatrolRandom)
        {
            if (AgentReachedDestination() == true)
            {
                StartCoroutine(SetBehaviourOnTime(AIBehaviour.Idle, currentPathNode.IdleTime));
            }
        }

        if (aIBehaviour == AIBehaviour.PatrolCircle)
        {
            if (AgentReachedDestination() == true)
            {
                StartCoroutine(SetBehaviourOnTime(AIBehaviour.Idle, currentPathNode.IdleTime));
            }
        }
    }

    private void ActionUpdateTarget()
    {
        if (potentialTarget == null) return;

        if (colliderViewer.IsObjectVisible(potentialTarget) || colliderViewer.IsObjectVisibleFromSide(potentialTarget))
        {
            OnVisibilityChanged?.Invoke(true);
            pursueTarget = potentialTarget.transform;
            ActionAssignTargetAllTeamMember(pursueTarget);

            //StartBehaviour(AIBehaviour.PursueTarget);
        }
        else
        {
            OnVisibilityChanged?.Invoke(false);

            if (pursueTarget != null)
            {
                seekTarget = pursueTarget.position;
                pursueTarget = null;
                StartBehaviour(AIBehaviour.FindNearestPoint);
            }
        }
    }

    IEnumerator SetBehaviourOnTime(AIBehaviour state, float second)
    {
        AIBehaviour previous = aIBehaviour;
        aIBehaviour = state;
        StartBehaviour(aIBehaviour);

        yield return new WaitForSeconds(second);

        StartBehaviour(previous);
    }

    private void ActionAssignTargetAllTeamMember(Transform other)
    {
        List<Destructible> team = Destructible.GetAllTeamMember(alienSoldier.TeamID);

        foreach (Destructible dest in team)
        {
            AIAlienSoldier ai = dest.transform.root.GetComponent<AIAlienSoldier>();

            if (ai != null && ai.enabled)
            {
                ai.SetPursueTarget(other);
                ai.StartBehaviour(AIBehaviour.PursueTarget);
            }
        }
    }

    private void SetPursueTarget(Transform target)
    {
        pursueTarget = target;
    }

    private void StartBehaviour(AIBehaviour state)
    {
        if (alienSoldier.IsDeath) return;

        if (state == AIBehaviour.Idle)
        {
            agent.isStopped = true;
            characterMovement.UnAiming();
        }

        if (state == AIBehaviour.PatrolRandom)
        {
            agent.isStopped = false;
            characterMovement.UnAiming();
            SetDestinationByPathNode(patrolPath.GetRandomPathNode());
        }

        if (state == AIBehaviour.PatrolCircle)
        {
            agent.isStopped = false;
            characterMovement.UnAiming();
            SetDestinationByPathNode(patrolPath.GetNextPathNode(ref patrolPathNodeIndex));
        }

        if (state == AIBehaviour.PursueTarget)
        {
            agent.isStopped = false;
        }

        if (state == AIBehaviour.SeekTarget)
        {
            agent.isStopped = false;
            characterMovement.UnAiming();
        }

        aIBehaviour = state;
    }

    private void SetDestinationByPathNode(PatrolPathNode node)
    {
        currentPathNode = node;
        agent.CalculatePath(node.transform.position, navMeshPath);
        agent.SetPath(navMeshPath);
    }

    private bool AgentReachedDestination()
    {
        if (agent.pathPending == false)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (agent.hasPath == false || agent.velocity.sqrMagnitude == 0.0f)
                {
                    return true;
                }

                return false;
            }

            return false;
        }

        return false;
    }

    private void SyncAgentAndCharacterMovement()
    {
        agent.speed = characterMovement.CurrentSpeed;

        float factor = agent.velocity.magnitude / agent.speed;
        characterMovement.TargetDirectionControl =
            transform.InverseTransformDirection(agent.velocity.normalized) * factor;
    }

    private Vector3 GenerateRandomPositionAround(Vector3 center)
    {
        float randomX = Random.Range(center.x - findRange, center.x + findRange);
        float randomZ = Random.Range(center.z - findRange, center.z + findRange);
        return new Vector3(randomX, center.y, randomZ);
    }
}