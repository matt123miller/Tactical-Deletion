using UnityEngine;
using System.Collections;

public class ventCrouchToggle : MonoBehaviour {

    private CamCallToggle cct;
    private GameObject gsmGO;

    void Start()
    {
        gsmGO = GameObject.FindWithTag("GSM");
        cct = gsmGO.GetComponent<CamCallToggle>();
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            //Debug.Log("in Collider");
            cct.ToggleCall();
        }
    }

	}

