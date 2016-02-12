using UnityEngine;
using System.Collections.Generic;

/*
TO DO:
Change agent speed during Patrol to walking.


*/
public class PatrolState : IEnemyState {

    private readonly AIManager aim;
    private NavMeshAgent navAgent;
    public List<Transform> waypoints = new List<Transform>();
    private float iRedirectDistance = 2f, suspicionValue, suspicionModifier = 0.1f;
    private int nextWaypoint = 0;

    public PatrolState(AIManager statepatternenemy, NavMeshAgent agent, Transform[] points)
    {

        aim = statepatternenemy;
        navAgent = agent;
        
        waypoints.AddRange(points);

       
    }

    public void UpdateState()
    {
        Patrol();

        if (aim.InSight)
        {
            
            if(suspicionValue <= 1)
            {
                suspicionValue += suspicionModifier;
                Debug.Log(suspicionValue);
            }
            else
            {
                GlobalStateManager.GSMInstance.SetStateSpotted();

                ToChaseState();
            }
            
            
        }
        else
        {
            if (suspicionValue > 0)
            {
                suspicionValue -= suspicionModifier / 2;
                Debug.Log(suspicionValue);
            }
        }

        // Should be handled by the observer pattern
        //else if ((GlobalStateManager.GSMInstance.GSMState == WorldState.alarmState) && !aim.InSight)
        //{
        //    AIState = AIStates.personalAlarmed;
        //    //unsureAlarmed = true;
        //    //cooldownTimer = 0f; //maybe
        //}
    }
    

   

    private void Patrol()
    {
        //Waypoint system
        if (navAgent.remainingDistance <= navAgent.stoppingDistance && !navAgent.pathPending)
        {
            nextWaypoint = (nextWaypoint + 1) % waypoints.Count;
        }

        // can this be moved inside the if?
        navAgent.destination = waypoints[nextWaypoint].position;
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
        suspicionValue = 0f;
        aim.SetAnimRunning();
    }

    public void ToChaseState()
    {
        aim.currentState = (IEnemyState)aim.chaseState;
        suspicionValue = 0f;
        aim.SetAnimRunning();

    }

    public void ToAttackState()
    {
        aim.currentState = (IEnemyState)aim.attackState;
        suspicionValue = 0f;
        aim.SetAnimRunning();

    }
}
