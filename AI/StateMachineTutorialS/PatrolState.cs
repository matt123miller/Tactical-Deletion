using UnityEngine;
using System.Collections.Generic;

/*
TO DO:

    
*/
public class PatrolState : IEnemyState
{

    private AIManager aim;
    private NavMeshAgent navAgent;
    public List<Transform> waypoints = new List<Transform>();
    private float redirectDistance = 0.5f, suspicionValue, suspicionModifier = 0.1f;
    private int nextWaypoint = 0;


    void Start()
    {
        aim = GetComponent<AIManager>();
        navAgent = aim.navAgent;

        waypoints.AddRange(GetComponent<Waypoints>().waypointTargets);

        navAgent.stoppingDistance = 0.1f;
        redirectDistance = navAgent.stoppingDistance;
        navAgent.SetDestination(waypoints[nextWaypoint].position);
    }

    public PatrolState(AIManager statepatternenemy, NavMeshAgent agent, Transform[] points)
    {

        //aim = statepatternenemy;
        //navAgent = agent;
        
        //waypoints.AddRange(points);

        //aim.SetSpeedWalking();
    }

    override public void UpdateState()
    {
       Patrol();

        if (aim.InSight)
        {

            //if(suspicionValue <= 1)
            //{
            //    suspicionValue += suspicionModifier;
            //    Debug.Log(suspicionValue);
            //}
            //else
            //{
            //    GlobalStateManager.GSMInstance.SetStateSpotted();

            //    ToChaseState();
            //}

            //GlobalStateManager.GSMInstance.SetStateSpotted();

            ToChaseState();
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
        if (navAgent.remainingDistance < redirectDistance) // && !navAgent.pathPending
        {
            nextWaypoint = (nextWaypoint + 1) % waypoints.Count;
            navAgent.SetDestination(waypoints[nextWaypoint].position);
            navAgent.Resume();
        }

        // can this be moved inside the if?
        
    }

    override public void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag("Player"))
        //{
        //}   
    }

    override public void ToPatrolState()
    {
        Debug.Log("Can't transition to same state");
    }

    override public void ToAlertState()
    {

        Debug.Log("Patrol to Alert state");
        aim.currentState = aim.alertState;
        suspicionValue = 0f;
        childTimer = 0f;
        aim.SetSpeedRunning();
    }

    override public void ToChaseState()
    {
        Debug.Log("Patrol to Chase state");
        aim.currentState = (IEnemyState)aim.chaseState;
        suspicionValue = 0f;
        childTimer = 0f;

        // play a "huh?" sound

        aim.SetSpeedRunning();
    }

    override public void ToAttackState()
    {
        Debug.Log("Patrol to Attack State");
        aim.currentState = (IEnemyState)aim.attackState;
        suspicionValue = 0f;
        childTimer = 0f;
        aim.SetSpeedRunning();

    }
}
