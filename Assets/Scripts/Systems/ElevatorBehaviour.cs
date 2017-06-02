using UnityEngine;
using System.Collections;

public class ElevatorBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){

            SceneTransitionManager.Instance.LoadNextLevel();
        }
    }
}
