using UnityEngine;
using System.Collections;

public class AISight : MonoBehaviour
{


    RaycastHit rayHit;
    public AIManager aim;
    private bool inSight;
    float fUpdateChase = 0.3f; //
    float fUpdateCount = 0.0f;
    private GameObject goPlayer;
    Vector3 targetPos;

    [SerializeField]
    private Transform eyes;
    //public GameObject self;

    //public float selfAlarmedCheck, selfAlarmTimer;

    public NavMeshAgent agent { get; private set; }
    public ThirdPersonCharacter character { get; private set; }


    public bool InSight
    {
        get { return inSight; }
        set { inSight = value; }
    }

    public Transform Eyes
    {
        get { return eyes; }
        set { eyes = value; }
    }

    private void Start()
    {
        goPlayer = GameObject.FindWithTag("Player");

        //EnemySelf = ;
    }


    public AISight()
    {
    }

    void Update()
    {
    }


    void OnTriggerStay(Collider other)
    {
        if (fUpdateCount > fUpdateChase)
        {
            fUpdateCount = 0.0f;
            if (other.tag == "Player")
            {
                inSight = Look(other.transform.position);
            }
        }
        else
            fUpdateCount += Time.deltaTime;
    }

    public bool Look(Vector3 otherPosition)
    {


        Vector3 direction = otherPosition - eyes.position;
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(eyes.position, direction, out hit))
        {
            if (hit.collider.tag == "Player")
            {
                targetPos = hit.collider.gameObject.transform.position;
                aim.LKP = targetPos;

                aim.SpottedByEnemy();
                Debug.Log("Player is Sighted, player LKP updated " + aim.LKP);
                return true;
            }
        }
        else
        {
            Debug.Log("In collider, not in sight");
        }

        return false;
    }

    private void OnTriggerExit()
    {
        InSight = false;
    }
}

