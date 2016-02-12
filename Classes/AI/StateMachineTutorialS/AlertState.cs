using UnityEngine;
using System.Collections;

public class AlertState : MonoBehaviour, IEnemyState
{


    private readonly AIManager aim;

    public Transform playerTrans, alarmTrans;
    private GameObject[] alarmTransforms;
    private AudioSource shoutClip;
    //private Transform[] arrTranAlarms;
    public float searchTimer = 0f;

    public AlertState(AIManager aim)
    {
        this.aim = aim;
        shoutClip = aim.gameObject.GetComponent<AudioSource>();

    }


    public void UpdateState()
    {
        Search();
    }

    public void Shout()
    {
        Debug.Log("Shouting! RAWR!");
        shoutClip.Play();
    }

  


    private void Search()
    {
        // do some searching

        //if not in sight ? {
        //    searchTimer += Time.deltaTime;
        //    
        //    if searchTimer >= enemy.searchingDuration {
        //        ToPatrolState();
        //    }
        //}
    }

    private bool AlarmSearch()
    {
        alarmTrans = GetClosestAlarm(alarmTransforms);

        if (alarmTrans == null)
            return false;
        else
        {
            Debug.Log("Might move to " + alarmTrans);

            return true; //moves to alarm, autoused with right states
        }
    }

    //Not ideal solution, see below, however it's the best I have for now.
    private Transform GetClosestAlarm(GameObject[] alarms)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;

        foreach (GameObject potentialTarget in alarms)
        {
            float f = Vector3.Distance(potentialTarget.transform.position, transform.position);
            if (f < closestDistanceSqr)
            {
                if (f < 10f)
                    bestTarget = potentialTarget.transform;
            }
        }
        return bestTarget;
    }

    //Currently not working, it is the ideal solution but navmeshpaths take time to calculate
    //This is bad. For now I am just finding the closest using Vector3.Distance()
    private Transform GetClosestAlarmPath(GameObject[] alarms)
    {
        //use navmesh remaining distance

        NavMeshPath path = new NavMeshPath();

        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject potentialTarget in alarms)
        {

            //NavMeshPath pathToTarget = NavMesh.CalculatePath(currentPosition, potentialTarget.transform.position, NavMesh.AllAreas, path);

            // agent.SetPath(pathToTarget);
            aim.navAgent.SetDestination(potentialTarget.transform.position);

            if (aim.navAgent.remainingDistance < closestDistanceSqr)
            {
                //problem is here, agent.remainingdistance doesn't seem to work
                closestDistanceSqr = aim.navAgent.remainingDistance;
                bestTarget = potentialTarget.transform;
            }

            //float distToTarget = 0;
            //Vector3[] corners = pathToTarget.corners;

            //for (int i = 0; i < corners.Length; i++)
            //{
            //    distToTarget += Vector3.Distance(corners[i], corners[i + 1]);
            //}
        }
        Debug.Log(bestTarget.name);
        return bestTarget;
    }


    public void OnTriggerEnter(Collider other)
    {

    }

    public void ToPatrolState()
    {
        aim.currentState = (IEnemyState)aim.patrolState;
        searchTimer = 0f;
        aim.SetAnimWalking();

    }

    public void ToAlertState()
    {
        Debug.Log("Can't transition to same state");

    }

    public void ToChaseState()
    {
        aim.currentState = (IEnemyState)aim.chaseState;
        searchTimer = 0f;
    }

    public void ToAttackState()
    {
        aim.currentState = (IEnemyState)aim.attackState;
        searchTimer = 0f;
    }
}
