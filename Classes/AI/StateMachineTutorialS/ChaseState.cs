using UnityEngine;
using System.Collections;
using System;

public class ChaseState : MonoBehaviour, IEnemyState {


    private readonly AIManager aim;

    private Transform playerTrans;
    private float minChaseDistance, maxChaseDistance;

    public ChaseState(AIManager statepatternenemy, Transform player)
    {
        aim = statepatternenemy;
        playerTrans = player;
    }


    public void UpdateState()
    {
        
        if (aim.InSight)
        {
            float enemyToTarget = (playerTrans.position - transform.position).magnitude;

            if (enemyToTarget < minChaseDistance)
            {
                ToAttackState();
            }
            else if (enemyToTarget > maxChaseDistance)
            {
                ToAlertState();
            }

            return;
        }
        else 
        {
            ToAlertState();
            return;
        }

        Chase();
    }

    
    private void Chase()
    {
        aim.navAgent.destination = playerTrans.position;
        aim.navAgent.Resume();

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

    
}
