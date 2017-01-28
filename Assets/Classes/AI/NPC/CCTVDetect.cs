using System.Linq.Expressions;
using UnityEngine;
using System.Collections;

public class CCTVDetect : MonoBehaviour
{

    private GameObject player;
    private static GlobalStateManager gsm;
    // Why?
    private AIManager aimanage;
    public bool inView = false;
    public bool inCollider = false;
    public float timerTarget, timerCounter;


    private void Start()
    {

        gsm = GlobalStateManager.GSMInstance;
        aimanage = GetComponent<AIManager>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            inCollider = true;
        }
    }

    void Update()
    {
        if (inCollider)
        {
            if (timerCounter > timerTarget)
            {
                timerCounter = 0f;
                Look();
            }
            else
            {
                timerCounter += Time.deltaTime;
            }
        }
    }

    private void Look()
    {
        Debug.Log("Ray sent out");

        Vector3 relativePlayerPos = player.transform.position - transform.position;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, relativePlayerPos, out hit))
        {
            inView = true;
            gsm.CamSpotted(hit.transform.position);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inCollider = false;
            timerCounter = 0f;

            if (inView)
            {
                inView = false;
                gsm.StartAlarmTimer();
            }
        }

    }
}
