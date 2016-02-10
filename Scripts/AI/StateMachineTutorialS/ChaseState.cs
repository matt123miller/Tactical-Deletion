using UnityEngine;
using System.Collections;
using System;

public class ChaseState : MonoBehaviour, IEnemyState {


    private readonly AIManager aim;
    

    public ChaseState(AIManager statepatternenemy)
    {
        aim = statepatternenemy;
    }


    public void UpdateState()
    {
        Look();
        Chase();
    }

    private void Look()
    {
        RaycastHit hit;
        // Used to look directly at target insteazd of forward
        Vector3 enemyToTarget = (aim.chaseTarget.position + aim.offset) - aim.eyes.transform.position;

        if (Physics.Raycast(aim.eyes.transform.position, enemyToTarget, out hit, aim.sightRange) && hit.collider.CompareTag("Player"))
        {
            aim.chaseTarget = hit.transform;
            ToChaseState();
        }
        else
        {
            // Maybe then wrap in a timer or some condition
            ToAlertState();
        }
    }

    private void Chase()
    {
        aim.navMeshAgent.destination = aim.chaseTarget.position;
        aim.navMeshAgent.Resume();

        // If close enough then move to 
    }

    public void OnTriggerEnter(Collider other)
    {

    }

    public void ToPatrolState()
    {
        aim.currentState = (IEnemyState)aim.patrolState;

    }

    public void ToAlertState()
    {
        aim.currentState = (IEnemyState)aim.chaseState;

    }

    public void ToChaseState()
    {
        Debug.Log("Can't transition to same state");

    }

    public void ToAttackState()
    {
        aim.currentState = (IEnemyState)aim.attackState;

    }

    //wtf?
    public static implicit operator Transform(ChaseState v)
    {
        throw new NotImplementedException();
    }
}
