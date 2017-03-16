/*
    AIManager is in charge of regulating various states.


*/


using UnityEngine;
using System.Collections;


public enum AIStates
{
    patrolling,
    personalAlarmed,
    attackState,
    alarmSearching,
    unsureAlarmed
}

[RequireComponent(typeof(ThirdPersonCharacter))]
[RequireComponent(typeof(ChaseState))]
[RequireComponent(typeof(PatrolState))]
[RequireComponent(typeof(AlertState))]
[RequireComponent(typeof(AttackState))]
public class AIManager : IObserver
{

    // Introduce FSM
    //[HideInInspector]
    public IEnemyState currentState;
    [HideInInspector]
    public ChaseState chaseState;
    [HideInInspector]
    public AlertState alertState;
    [HideInInspector]
    public PatrolState patrolState;
    [HideInInspector]
    public AttackState attackState;

    // Classes
    private AIStates aiState;

    private static GlobalStateManager gsm;
    private AISight aiSight;
    private Health selfHealth;
    public ThirdPersonCharacter character { get; private set; }
    public NavMeshAgent navAgent;

    [SerializeField]
    private bool inSight;

    private float walkSpeed = 0.5f, alertSpeed = 0.75f, runSpeed = 1f;

    private Transform playerTransform;
    private Vector3 resetPosition = new Vector3(1000.0f, 1000.0f, 1000.0f);
    [SerializeField]
    private Vector3 lastKnownPos, targetPos;

    [SerializeField]
    private float cooldownTarget, cooldownTimer, suspicionValue;

    #region Properties

    // Last known position may or may not be where the player is now.
    // State classes should take that into account and maybe not assume that's where the player definitely is.
    public Vector3 LKP
    {
        get { return lastKnownPos; }
        set { lastKnownPos = value; targetPos = value; }
    }

    public AIStates AIState
    {
        get { return aiState; }
        set { aiState = value; }
    }

    public bool InSight
    {
        get { return inSight; }
        set { inSight = value; }
    }

    public Transform PlayerTransform
    {
        get { return playerTransform; }
        set { playerTransform = value; }
    }
    #endregion

    // Use this for initialization
    void Awake()
    {
        
        AIState = AIStates.patrolling;

        aiSight = GetComponentInChildren<AISight>();
        aiSight.aim = this;
        aiSight.Eyes = gameObject.transform;

        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        navAgent = GetComponentInChildren<NavMeshAgent>();
        character = GetComponent<ThirdPersonCharacter>();
        selfHealth = GetComponent<Health>();

        // FSM

        chaseState = GetComponent<ChaseState>();
        alertState = GetComponent<AlertState>();
        patrolState = GetComponent<PatrolState>();
        attackState = GetComponent<AttackState>();
        //chaseState = new ChaseState(this, playerTransform);
        //alertState = new AlertState(this);
        //patrolState = new PatrolState(this, navAgent, GetComponent<Waypoints>().waypointTargets);
        //attackState = new AttackState(this);

        currentState = patrolState;

        SetSpeedWalking();
    }

    void Start()
    {
        gsm = GlobalStateManager.GSMInstance;
        gsm.Subscribe(this);
    }


    override public void UpdateThisObserver(WorldState newState)
    {
        switch (newState)
        {

            case WorldState.alarmState:
                currentState.ToAlertState();
                break;

            case WorldState.ambientState:
                currentState.ToPatrolState();
                Reset();
                break;

            case WorldState.spottedState:
                currentState.ToChaseState();
                break;

            // Moving straight to attack state is not yet supported as in theory alert and chase can both move to attack 
            // assuming certain conditions are met.
            default:
                break;
        }
        // Maybe this IF should be changed/removed?
        if (AIState != AIStates.personalAlarmed)
        {

            Reset();
        }
    }


    public void Reset()
    {

        currentState.ToPatrolState();
        lastKnownPos = gsm.ResetPosition;
    }

    public void EnemySpotsPlayer()
    {
        if (AIState != AIStates.personalAlarmed)
        {

            Debug.Log("Spotted!");
            gsm.SetStateSpotted();
        }
        AIState = AIStates.personalAlarmed;

        currentState.ToChaseState();
    }

    public void PlayerInVision(Vector3 playerPosition)
    {
        lastKnownPos = playerPosition;
        inSight = true;

    }


    public void PlayerLeftVision()
    {
        inSight = false;
        // Maybe some other things?
    }


    public void SetSpeedWalking()
    {
        navAgent.speed = walkSpeed;
    }

    public void SetSpeedAlert()
    {
        navAgent.speed = alertSpeed;
    }

    public void SetSpeedRunning()
    {
        navAgent.speed = runSpeed;
    }

    // Update is called once per frame
    void Update()
    {

        #region Boilerplate for AIM 
        if (PlayerTransform != null)
        {
            // update the agents posiiton 
            navAgent.transform.position = transform.position;

            //float walkMultiplier = (walkToggle ? 1 : 0.5f);
            //move *= walkMultiplier;
            // use the values to move the character;
            character.Move(navAgent.desiredVelocity, false, false);
        }
        else
        {
            // We still need to call the character's move function, but we send zeroed input as the move param.
            character.Move(Vector3.zero, false, false);
        }
        #endregion

        currentState.UpdateState();

        //Might move a version of this to each state?
        //AI cooldown to normal
        if ((currentState != patrolState) && !inSight)
        {
            if ((gsm.GSMState == WorldState.spottedState))
            {
                if ((cooldownTimer += Time.deltaTime) > cooldownTarget)
                {
                    Reset();
                    cooldownTimer = 0f;
                }
            }
        
        }
        else
            cooldownTimer = 0f;
    }
}

