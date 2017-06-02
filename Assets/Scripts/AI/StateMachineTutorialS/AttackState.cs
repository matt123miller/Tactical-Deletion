using UnityEngine;
using System.Collections;

/*
    TODO:
    Once you come close to attcak make sure you carry on attacking if the player escapes or something
*/

public class AttackState : IEnemyState
{


    private AIManager aim;

    private Shoot shoot;


    void Start()
    {
        aim = GetComponent<AIManager>();
        shoot = GetComponent<Shoot>();
    }

    //public AttackState(AIManager statepatternenemy)
    //{
    //    aim = statepatternenemy;
    //}


    // Update is called once per frame
    public override void UpdateState()
    {
        // assumes you're in sight
        if (MovingToAttack() == false)
        {
            Attack();
        }
        // Therefore here you're not in sight
        else
        {
            if (TimerTick())
            {
                ToAlertState();
            }
        }
    }

    private bool MovingToAttack()
    {
        // Debug.Log("attacking");
        if (aim.InSight) {

            if ((aim.LKP - transform.position).magnitude < 10f) {

                aim.navAgent.destination = transform.position;
                aim.navAgent.Stop();
                Debug.Log("close enough to attack");
                return false;
            }
            else {

                aim.navAgent.destination = aim.LKP;
            }
        }
        else {

            aim.navAgent.destination = aim.LKP;
        }

        aim.navAgent.Resume();

        // Maybe there needs to be more return falses later when this area expands.
        return true;

    }

    private void Attack()
    {
        // This is probably slow, any way to speed it up?
        shoot.Shooting();

    }

    override public void OnTriggerEnter(Collider other)
    {

    }

    override public void ToPatrolState()
    {
        Debug.Log("Attack to Patrol state");
        aim.currentState = (IEnemyState)aim.patrolState;
        aim.SetSpeedWalking();

    }

    override public void ToAlertState()
    {
        Debug.Log("Attack to Alert state");
        aim.currentState = (IEnemyState)aim.chaseState;
        aim.SetSpeedAlert();

    }

    override public void ToChaseState()
    {
        Debug.Log("Attack to Chase state");
        aim.currentState = (IEnemyState)aim.attackState;
        aim.SetSpeedRunning();
    }

    override public void ToAttackState()
    {
        Debug.Log("Can't transition to same state");

    }
}
