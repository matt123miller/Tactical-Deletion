using UnityEngine;
using System.Collections;
using System;

public abstract class IEnemyState : MonoBehaviour
{

    public float childTimer, childTimerTarget = 5;

    protected bool TimerTick()
    {
        if ((childTimer += Time.deltaTime) >= childTimerTarget){

            childTimer = 0f;
            return true;
        }
        return false;
    }


    abstract public void UpdateState();

    abstract public void OnTriggerEnter(Collider other);

    abstract public void ToPatrolState();

    abstract public void ToAlertState();

    abstract public void ToChaseState();

    abstract public void ToAttackState();
    
}