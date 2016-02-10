using UnityEngine;
using System.Collections;

public class AlarmPanel : MonoBehaviour
{


    private GlobalStateManager gsm;
    private bool inTrigger;
    // Use this for initialization
    void Start()
    {
        gsm = GlobalStateManager.GSMInstance;
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Enemy"))
        {
            Debug.Log("Enemy is in collider");
            AIManager aim = coll.GetComponent<AIManager>();
            
            if (aim.AIState == AIStates.personalAlarmed)
            {
                //aic.BusyBool = false;

                Debug.Log("Enemy is mad and entered! Rawr!");
                if (gsm.GSMState != WorldState.alarmState)
                {
                    Debug.Log("Enemy is mad alarm is off, turning it on now");
                    gsm.SetStateAlarm();
                    gsm.StartAlarmTimer();
                }

            }

        }
        
        if (coll.CompareTag("Player")) {
            
            inTrigger = true;
        }
    }

    private void OnTriggerExit() {
        
        if (inTrigger) {
            
            inTrigger = false;
        }
    }
    private void OnTriggerStay(Collider coll)
    {
        if (inTrigger) {
            
            if (Input.GetButtonDown("Interact") && gsm.GSMState == WorldState.alarmState) {
                
                Debug.Log("AlarmOff");
                gsm.UpdateThisObserver(WorldState.alarmState);
                //gsm.GSMState = WorldState.alarmState;
            }
            //if (Input.GetButtonDown("Interact") && !gsm.AlarmState)
            //maybe play noise for false interact
        }
    }
    
}
