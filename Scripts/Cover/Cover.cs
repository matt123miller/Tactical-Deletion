using UnityEngine;
using System.Collections;

public class Cover : MonoBehaviour {

	public GameObject player;
	private bool coverB = false;
	
void OnTriggerEnter(Collider other){

	    if (Input.GetKeyUp(KeyCode.Mouse1) && other == player) //mouse1 is right click
	    {
			Debug.Log ("right click");
			coverB = !coverB;
			GetComponent<Collider>().isTrigger = !GetComponent<Collider>().isTrigger;
	    }

	}
}


//near object, press button for cover
//make player child of cover object
//create a collider around the cover to keep the player in it
//press button again to unmake child of cover object, destroy the collider