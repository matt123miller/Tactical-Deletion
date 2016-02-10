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

public class AIManager : IObserver
{

    // Introduce FSM
    [HideInInspector]
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
    private WorldState worldState;
    public ThirdPersonCharacter character { get; private set; }
    public AISight aiSight;
    private static GlobalStateManager gsm;
    private Health selfHealth, playerHealth;

    public NavMeshAgent navAgent;

    public Transform playerTrans;
    private Vector3 resetPosition = new Vector3(1000.0f, 1000.0f, 1000.0f);
    [SerializeField]
    private Vector3 lastKnownPos, previousSighting, targetPos;

    [SerializeField]
    private float cooldownCheck, cooldownTimer, timer;

    public Vector3 LKP
    {
        get { return lastKnownPos; }
        set { lastKnownPos = value; }
    }

    public AIStates AIState
    {
        get { return aiState; }
        set { aiState = value; }
    }


    // Use this for initialization
    void Awake()
    {
        gsm = GlobalStateManager.GSMInstance;
        AIState = AIStates.patrolling;

        aiSight = GetComponentInChildren<AISight>();
        aiSight.aim = this;
        aiSight.Eyes = gameObject.transform;
        
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
        navAgent = GetComponentInChildren<NavMeshAgent>();
        character = GetComponent<ThirdPersonCharacter>();
        selfHealth = GetComponent<Health>();

        chaseState = new ChaseState(this);
        alertState = new AlertState(this);
        patrolState = new PatrolState(this, navAgent);
        attackState = new AttackState(this);

    }

    void Start()
    {

        gsm.Subscribe(this);
    }


    override public void UpdateThisObserver(WorldState newState)
    {
       
       if (AIState != AIStates.personalAlarmed) {
           
           Reset();
       }
    }


    public void Reset()
    {

        AIState = AIStates.patrolling;
        lastKnownPos = gsm.ResetPosition;
    }

    public void SpottedByEnemy()
    {
        if (AIState != AIStates.personalAlarmed) { 

            Debug.Log("Spotted!");
            gsm.SetStateSpotted();
        }
        AIState = AIStates.personalAlarmed;

        currentState.ToChaseState();
    }

    // Update is called once per frame
    void Update()
    {

        #region
        if (playerTrans != null)
        {
            // update the agents posiiton 
            navAgent.transform.position = transform.position;

            //float walkMultiplier = (walkToggle ? 1 : 0.5f);
            //move *= walkMultiplier;
            // use the values to move the character;
            character.Move(navAgent.desiredVelocity, false, false, targetPos);
        }
        else
        {
            // We still need to call the character's move function, but we send zeroed input as the move param.
            character.Move(Vector3.zero, false, false, transform.position + transform.forward * 100);
        }
        #endregion

        
        currentState.UpdateState();

        // Check AISight for current sight status, do stuff.


        if ((gsm.GSMState == WorldState.alarmState) && (AIState != AIStates.personalAlarmed) && !aiSight.InSight)
        {
            AIState = AIStates.personalAlarmed;
            //unsureAlarmed = true;
            //cooldownTimer = 0f; //maybe
        }

        //if ((AIState == AIStates.personalAlarmed) && aiSight.InSight)
        //{
        //    //do something aggressive
        //    //maybe this is handled in AIController
        //}

        //AI cooldown to normal
        //if (!aisight.InSight && !gsm.AlarmState && (personalAlarmed || unsureAlarmed) && !alarmSearching)
        if ((gsm.GSMState != WorldState.alarmState) && (AIState == AIStates.personalAlarmed) && !aiSight.InSight)
        {
            cooldownTimer += Time.deltaTime;

            if (cooldownTimer > cooldownCheck)
            {
                Reset();
                cooldownTimer = 0f;
            }
        }
        else
            cooldownTimer = 0f;
    }
}

