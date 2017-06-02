using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LockedDoor : MonoBehaviour {
	//Welcome to the included locked door script! Herein are all the bells and whistles of the doors' behaviours.
	//The script is set up in such a way that you can call upon the Open/Close function from a custom script,
	//meaning that you can change how you interact with the doors, if you know your way around basic scripting.
	//As a sample, this script includes behaviour for opening and closing, when a character controller is intersecting the trigger of the door.
	//This variant also includes a function to call upon for 'unlocking' the door (it is locked by default.)
	//Happy developing! /Marcus S.
	
	public GameObject doorChild; //The door body child of the door prefab.
	public GameObject audioChild; //The prefab's audio source GameObject, from which the sounds are played.
	
	public AudioClip openSound; //The door opening sound effect (3D sound.)
	public AudioClip closeSound; //The door closing sound effect (3D sound.)
	public AudioClip lockedSound; //Sound to play when trying to open a locked door.
	public AudioClip lockingSound; //Sound to play when locking/unlocking.
	
	private bool inTrigger = false; //Bool to check if CharacterController is in the trigger.
	private bool doorOpen = false; //Bool used to check the state of the door, if it's open or not.
	private bool doorLocked = true; //Bool used to check if door is locked.

    private List<KeyCard> keycards = new List<KeyCard>();
    public KeyCard.KeyColour keyColour;

    
	//Door opening and closing function. Can be called upon from other scripts.
	public void doorOpenClose() {
		//First check if the door is locked. If not, go ahead and open/close. Otherwise, play "locked" sound.
		if (doorLocked == false) {
			//Check so that we're not playing an animation already.
			if (doorChild.GetComponent<Animation>().isPlaying == false) {
				//Check the state of the door, to determine whether to close or open.
				if (doorOpen == false) {
					//Opening door, play Open animation and sound effect.
					doorChild.GetComponent<Animation>().Play("Open");
					audioChild.GetComponent<AudioSource>().clip = openSound;
					audioChild.GetComponent<AudioSource>().Play();
					doorOpen = true;
				}
				else {
					//Closing door, play Close animation and sound effect.
					doorChild.GetComponent<Animation>().Play("Close");
					audioChild.GetComponent<AudioSource>().clip = closeSound;
					audioChild.GetComponent<AudioSource>().Play();
					doorOpen = false;
				}
			}
		}
		else if (doorLocked == true) {
			audioChild.GetComponent<AudioSource>().clip = lockedSound;
			audioChild.GetComponent<AudioSource>().Play();
		}
	}
	
	
	
	//Function for unlocking/locking the door, thus enabling/disabling it to be opened and closed.
	//Use toggleDoorLock(true) to lock the door and toggleDoorLock(false) to unlock it.
	public void toggleDoorLock(bool toggleLocked) {
		//First check that the door isn't already open or animating. It would be counter productive to lock an open door, right?
		if (doorOpen == false && doorChild.GetComponent<Animation>().isPlaying == false) {
			doorLocked = toggleLocked;
			audioChild.GetComponent<AudioSource>().clip = lockingSound;
			audioChild.GetComponent<AudioSource>().Play();
		}
	}
	
	public bool HasRequiredKeycard()
    {

        foreach (var keycard in keycards) {
          
            if (keycard.keycolour == keyColour)
            {
                toggleDoorLock(false);
                return true;
            }
        }
        //toggleDoorLock(false);
        return false;
    }
	
	//The rest is for the interaction with the door. This can be removed or altered if you'd like to control the doors in a different way.
	//Set the inTrigger to true when CharacterController is intersecting, which in turn means routine in Update will check for button press (interaction.)
	void OnTriggerEnter(Collider collider) {
	    if (collider.CompareTag("Player"))
	    {
	        inTrigger = true;
	        keycards = collider.GetComponent<PlayerInventory>().Keycards;
	    }
        else if (collider.CompareTag("Enemy"))
        {
            inTrigger = true;
            keycards = collider.GetComponent<EnemyInventory>().Keycards;
            if (HasRequiredKeycard())
            {
                doorOpenClose();
            }
        }
	}
	//Set the inTrigger to false when CharacterController is out of trigger, which in turn means routine in Update will NOT check for button press.
	void OnTriggerExit(Collider collider) {
		if (collider.CompareTag("Player") || collider.CompareTag("Enemy"))
        {
            inTrigger = false;
            toggleDoorLock(true);
        }
			
	}
	
	void Update() {
		//Check the inTrigger bool, to see if CharacterController is in the trigger and thus can interact with the door.
		if (inTrigger == true) {
			//If inTrigger is true, check for button press to interact with door.
			//For this sample behaviour, we're checking for Fire2, which defaults to the right mouse button.
            if (Input.GetButtonDown("Interact"))
            {
                if (HasRequiredKeycard())
                {
                        doorOpenClose();
                }
			}
			//LOCKED DOOR: As an example to unlock the door, the player can press Fire1 (default Left Mouse Button) to toggle unlocked/locked.
            //if (Input.GetButtonDown("Interact"))
            //{
            //    //Check the current lock state to determine to lock or unlock the door.
            //    if (doorLocked == true) {

            //        toggleDoorLock(false);
            //    }
            //    else if (doorLocked == false) {
            //        toggleDoorLock(true);
            //    }
            //}
		}
	}
}
