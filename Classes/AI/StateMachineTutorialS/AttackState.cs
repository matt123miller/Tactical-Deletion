using UnityEngine;
using System.Collections;

public class AttackState : MonoBehaviour, IEnemyState
{


    private readonly AIManager aim;

    private Shoot shoot;

    public AttackState(AIManager statepatternenemy)
    {
        aim = statepatternenemy;
        shoot = new Shoot();
    }


    // Use this for initialization
    void Start()
    {

    }

    private void Attack()
    {
        Debug.Log("attacking");
        if ((Vector3.Distance(aim.LKP, transform.position) < 10f) && (aim.InSight))
        {
            aim.navAgent.Stop();
            Debug.Log("close enough to attack");
            shoot.Shooting();
        }
        else
            aim.navAgent.destination = aim.LKP;
    }

    // Update is called once per frame
    public void UpdateState()
    {

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
        aim.currentState = (IEnemyState)aim.attackState;

    }

    public void ToAttackState()
    {
        Debug.Log("Can't transition to same state");

    }
}
