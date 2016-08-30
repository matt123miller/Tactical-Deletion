using UnityEngine;
using System.Collections;

/*
    TODO:
    AI can see through walls!?!?!?! WHY?
*/

public class AISight : MonoBehaviour
{


    RaycastHit hit;
    public AIManager aim;
    private bool inSight;
    public float fUpdateTarget = 0.2f;
    public float fUpdateTimer = 0.0f;
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
        // Start each agents timer on a semi random time, why not? 
        // Should space out the bigger computations across different frames instead of all agents in 1 frame
        fUpdateTimer += Random.Range(-0.1f, 0.1f);
    }
    

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if ((fUpdateTimer += Time.deltaTime) > fUpdateTarget)
            {
                fUpdateTimer = 0.0f;
                Look(other.transform.position);
            }
        }
    }

    public bool Look(Vector3 otherPosition)
    {
        Vector3 direction = otherPosition - eyes.position;

        hit = new RaycastHit();

        Debug.DrawRay(transform.position, direction, Color.green);
        if (Physics.Raycast(eyes.position, direction, out hit))
        {
            if (hit.collider.CompareTag("Player"))
            {
                inSight = true;
                aim.PlayerInVision(hit.transform.position);
                Debug.Log("Player is Sighted, player LKP updated");
            }
        }
        else
        {
            inSight = false;
            Debug.Log("In collider, not in sight");
        }

        return inSight;
    }

    private void OnTriggerExit(Collider other)
    {
        if (inSight && other.CompareTag("Player"))
        {
            // if this ever occurs and the player IS in sight then there's a problem
            // if not delete this breakpoint
            // Breakpoint removed for now, might add it agian later to test.
            aim.PlayerLeftVision();
        }
    }
}

