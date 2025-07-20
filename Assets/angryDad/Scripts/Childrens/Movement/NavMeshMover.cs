using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshMover : IChildMover
{
    private readonly NavMeshAgent _agent;

    public NavMeshMover(NavMeshAgent agent)
    {
        _agent = agent;
    }

    public void MoveTo(Vector3 position)
    {
        _agent.SetDestination(position);
    }

    public bool HasReachedDestination()
    {
        return !_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance;
    }
}
