
////using ProBuilder2.Common;
//using UnityEngine;
//using System.Collections;
//using UnityEngine.UI;
//using System.Linq;
//using ProBuilder2.Common;



//[RequireComponent(typeof(ThirdPersonCharacter))]
//public class AIController : MonoBehaviour
//{
//    // Classes
    
//    GlobalStateManager gsm;
//    ThirdPersonCharacter character;
//    AISight ais;
//    AIManager aim;
//    NavMeshAgent agent;

    
//    int nextWaypoint;

//    private Vector3 move;
//    public bool walkToggle = true;

//    public Transform playerTrans;
//    private Transform alarmTrans;
//    private GameObject[] alarmTransforms;
//    //private Transform[] arrTranAlarms;
//    private bool busyBool; //used to ensure AI focuses on current task instead of doing others.

//    public bool BusyBool
//    {
//        get { return busyBool; }
//        set { busyBool = value; }
//    }



//    // Use this for initialization
//    void Awake()
//    {
//        //get all components
//        gsm = GlobalStateManager.GSMInstance;
//        ais = GetComponentInChildren<AISight>();
//        aim = GetComponent<AIManager>();

//        aim.Reset();
//        ////assign each AI class the correct instances of other classes.
//        aim.aiSight = ais;
//        ais.aim = aim;
//        //ais.EnemySelf = gameObject;


//        alarmTransforms = GameObject.FindGameObjectsWithTag("AlarmPanel");

        

//        BusyBool = false;

//        character.MoveSpeedMultiplier = 0.6f;
//        character.AnimSpeedMultiplier = 0.75f;

//    }

//    // Update is called once per frame
//    private void Update()
//    {
       
//        //enemy logic
//        if (aim.AIState == AIStates.attackState)
//        {
//            //ideally...
//            //if(ais.InSight)
//            //shoot
//            //else
//            //move to LKP

//            Attack();
//        }

//        else if (aim.AIState == AIStates.personalAlarmed)
//        {
//            if (gsm.GSMState != WorldState.alarmState)
//            {
//                if (!busyBool && (aim.AIState == AIStates.alarmSearching))
//                {
//                    if (AlarmSearch())
//                    {
//                        aim.AIState = AIStates.alarmSearching;
//                        busyBool = true;
//                    }
//                }
//                if (aim.AIState == AIStates.alarmSearching)
//                {
//                    agent.destination = alarmTrans.position; //moves to alarm, autoused with right states
//                }
//            }

            
//            if ((aim.AIState != AIStates.alarmSearching) && (gsm.GSMState == WorldState.alarmState)) //gsm.AlarmState
//            {
//                //Alarm is greater than 5f away
//                aim.AIState = AIStates.attackState;
//            }
//            if ((aim.AIState != AIStates.alarmSearching) && (gsm.GSMState != WorldState.alarmState))
//            {
//                Shout();
//                agent.destination = aim.LKP;
//            }

//            //else if (!ais.InSight && aim.PersonalAlarmed && gsm.AlarmState)
//            //{
//            //    character.MoveSpeedMultiplier = 1f;
//            //    agent.destination = aim.LKP;
//            //    //some sort of search method near LKP
//            //}
//        }

//        if (gsm.GSMState == WorldState.ambientState)
//        {
//            character.MoveSpeedMultiplier = 0.6f;
//            //Waypoint();
//            busyBool = false;
//        }
//    }

  


    


    
//}

