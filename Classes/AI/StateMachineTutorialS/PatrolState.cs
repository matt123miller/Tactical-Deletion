using UnityEngine;
using System.Collections;

public class PatrolState : IEnemyState {

    private readonly AIManager aim;
    private NavMeshAgent navAgent;
    public GameObject[] waypoints;
    private int arrLength;
    private float iRedirectDistance = 2f;
    private int nextWaypoint;

    public PatrolState(AIManager statepatternenemy, NavMeshAgent agent)
    {
        aim = statepatternenemy;
        arrLength = waypoints.Length;
        navAgent = agent;

    }

    public void UpdateState()
    {
        Patrol();
    }
    

   

    private void Patrol()
    {
        //Waypoint system
        if (navAgent.remainingDistance <= navAgent.stoppingDistance && !navAgent.pathPending)
        {
            nextWaypoint = (nextWaypoint + 1) % waypoints.Length;
        }

        // can this be moved inside the if?
        navAgent.destination = waypoints[nextWaypoint].transform.position;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ToAlertState();
        }   
    }

    public void ToPatrolState()
    {
        Debug.Log("Can't transition to same state");
    }

    public void ToAlertState()
    {
        aim.currentState = (IEnemyState)aim.alertState;
    }

    public void ToChaseState()
    {
        aim.currentState = (IEnemyState)aim.chaseState;

    }

    public void ToAttackState()
    {
        aim.currentState = (IEnemyState)aim.attackState;

    }
}
