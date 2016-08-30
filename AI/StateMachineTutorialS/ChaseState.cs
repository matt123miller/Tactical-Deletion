using UnityEngine;
using System.Collections;
using System;

public class ChaseState : IEnemyState
{


    private AIManager aim;
    private Transform playerTrans;
    public float minChaseDistance = 8f, maxChaseDistance = 25f;


    void Start()
    {
        aim = GetComponent<AIManager>();
        playerTrans = aim.PlayerTransform;

    }
    public ChaseState(AIManager statepatternenemy, Transform player)
    {
        aim = statepatternenemy;
        playerTrans = player;
    }


    override public void UpdateState()
    {

        if (aim.InSight)
        {
            float enemyToTarget = (aim.LKP - transform.position).magnitude;

            if (enemyToTarget < minChaseDistance)
            {
                ToAttackState();
                return;
            }
            else if (enemyToTarget > maxChaseDistance)
            {
                ToAlertState();
                return;
            }
        }
        else
        {
            if (TimerTick())
            {
                ToAlertState();
                return;
            }
        }

        Chase();
    }


    private void Chase()
    {
        aim.navAgent.destination = aim.LKP;
        //aim.navAgent.Resume();

        // If close enough then move to 
    }

    override public void OnTriggerEnter(Collider other)
    {

    }

    override public void ToPatrolState()
    {
        Debug.Log("Chase to patrol state");
        aim.currentState = (IEnemyState)aim.patrolState;
        childTimer = 0f;
        aim.SetSpeedWalking();

    }

    override public void ToAlertState()
    {
        Debug.Log("Chase to alert state");
        aim.currentState = (IEnemyState)aim.alertState;
        childTimer = 0f;

    }

    override public void ToChaseState()
    {
        Debug.Log("Can't transition to same state");

    }

    override public void ToAttackState()
    {
        Debug.Log("Chase to Attack State");
        aim.currentState = (IEnemyState)aim.attackState;
        childTimer = 0f;

    }


}
