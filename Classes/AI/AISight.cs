using UnityEngine;
using System.Collections;

public class AISight : MonoBehaviour
{


    RaycastHit hit;
    public AIManager aim;
    private bool inSight;
    float fUpdateTarget = 0.2f;
    float fUpdateTimer = 0.0f;
    Vector3 targetPos;

    [SerializeField]
    private Transform eyes;

    public Transform Eyes
    {
        get { return eyes; }
        set { eyes = value; }
    }

    private void Start()
    {
    }


    public AISight()
    {
    }

    void Update()
    {
    }


    void OnTriggerStay(Collider other)
    {
        if (fUpdateTimer > fUpdateTarget)
        {
            fUpdateTimer = 0.0f;

            if (other.tag == "Player")
            {
                aim.InSight = Look(other.transform.position);
            }
        }
        else
        {
            fUpdateTimer += Time.deltaTime;
        }
    }

    public bool Look(Vector3 otherPosition)
    {


        Vector3 direction = otherPosition - eyes.position;

        hit = new RaycastHit();

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
        aim.InSight = false;
    }
}

