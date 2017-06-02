using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class AlertState : IEnemyState
{


    private AIManager aim;

    private Transform playerTrans, alarmTrans;
    private GameObject[] alarmTransforms;
    private NavMeshAgent navAgent;
    private AudioSource source;
    public AudioClip[] audioClips;



    void Start()
    {
        aim = GetComponent<AIManager>();
        navAgent = aim.navAgent;
        source = GetComponent<AudioSource>();
    }

    public AlertState(AIManager aim)
    {
        //this.aim = aim;
        //shoutClip = aim.gameObject.GetComponent<AudioSource>();

    }


    override public void UpdateState()
    {
        Search();

        if (aim.InSight)
        {
            Shout();
            ToChaseState();
        }
        else if (TimerTick())
        {
            // search for limited period of time
            // searches for quite a long time.
            ToPatrolState();
        }
    }

    public void Shout()
    {
        Debug.Log("Shouting! RAWR!");
        int chosenClip = (int)Random.Range(0, audioClips.Length);
        source.clip = audioClips[chosenClip];
        source.Play();
    }




    private void Search()
    {
        // do some searching somehow....
        
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
            float f = (potentialTarget.transform.position - transform.position).sqrMagnitude;
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

        UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();

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


    override public void OnTriggerEnter(Collider other)
    {

    }

    override public void ToPatrolState()
    {
        Debug.Log("Alert to Patrol state");
        aim.currentState = (IEnemyState)aim.patrolState;
        childTimer = 0f;
        aim.SetSpeedWalking();

    }

    override public void ToAlertState()
    {
        Debug.Log("Can't transition to same state");

    }

    override public void ToChaseState()
    {
        Debug.Log("Alert to Chase state");
        aim.currentState = (IEnemyState)aim.chaseState;
        childTimer = 0f;
    }

    override public void ToAttackState()
    {
        Debug.Log("Alert to Attack State");
        aim.currentState = (IEnemyState)aim.attackState;
        childTimer = 0f;
    }
}
