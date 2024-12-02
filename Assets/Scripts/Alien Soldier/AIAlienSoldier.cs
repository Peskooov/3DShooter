using System;
using System.Collections;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.AI;

public class AIAlienSoldier : MonoBehaviour
{
    public enum AIBehaviour
    {
        Null = 0,
        Idle = 1,
        PatrolRandom = 2,
        PatrolCircle = 3
    }

    [SerializeField] private AIBehaviour aIBehaviour;
    [SerializeField] private AlienSoldier alienSoldier;
    [SerializeField] private CharacterMovement characterMovement;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private PatrolPath patrolPath;
    [SerializeField] private int patrolPathNodeIndex = 0;

    private NavMeshPath navMeshPath;
    private PatrolPathNode currentPathNode;

    private void Start()
    {
        characterMovement.UpdatePosition = false;
        navMeshPath = new NavMeshPath();

        StartBehaviour(aIBehaviour);
    }

    private void Update()
    {
        SyncAgentAndCharacterMovement();
        UpdateAI();
    }

    private void UpdateAI()
    {
        if (aIBehaviour == AIBehaviour.Idle) return;
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

    IEnumerator SetBehaviourOnTime(AIBehaviour state, float second)
    {
        AIBehaviour previous = aIBehaviour;
        aIBehaviour = state;
        StartBehaviour(aIBehaviour);

        yield return new WaitForSeconds(second);

        StartBehaviour(previous);
    }

    private void StartBehaviour(AIBehaviour state)
    {
        if (state == AIBehaviour.Idle)
        {
            agent.isStopped = true;
        }

        if (state == AIBehaviour.PatrolRandom)
        {
            agent.isStopped = false;
            SetDestinationByPathNode(patrolPath.GetRandomPathNode());
        }

        if (state == AIBehaviour.PatrolCircle)
        {
            agent.isStopped = false;
            SetDestinationByPathNode(patrolPath.GetNextPathNode(ref patrolPathNodeIndex));
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
        float factor = agent.velocity.magnitude / agent.speed;
        characterMovement.TargetDirectionControl =
            transform.InverseTransformDirection(agent.velocity.normalized) * factor;
    }
}