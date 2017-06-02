using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DoorLock : MonoBehaviour
{
    // Successfully decoupled from door opening, useing Strategy Pattern.
    // All you need is a door that inherits from IDoor. That class handles all the opening and closing of the door.
    // This class controls whether the door is ALLOWED to open.


    private IDoor door; 
    private IInventory inventory;
    public KeyCard.KeyColour keyColour;

    public GameObject audioChild; 
    public AudioClip lockedSound; 
    public AudioClip lockingSound; 

    private bool inTrigger = false; 
    private bool doorOpen = false;
    public bool doorLocked = false;

    
    void Start()
    {
        door = gameObject.GetComponent<IDoor>();
    }
    

    //Set the inTrigger to true when CharacterController is intersecting, which in turn means routine in Update will check for button press (interaction.)
    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            inTrigger = true;
            inventory = collider.GetComponent<IInventory>();
        }
        if (collider.CompareTag("Enemy"))
        {
            inventory = collider.GetComponent<IInventory>();

            if (doorOpenClose(inventory))
                door.doorOpenClose();
        }
    }

    //Set the inTrigger to false when CharacterController is out of trigger, which in turn means routine in Update will NOT check for button press.
    void OnTriggerExit(Collider collider)
    {
        inventory = null;

        if (collider.gameObject.tag == "Player")
            inTrigger = false;
        if (collider.gameObject.tag == "Enemy")
        {
            door.doorOpenClose();
        }
    }


    void Update()
    {
        //Check the inTrigger bool, to see if CharacterController is in the trigger and thus can interact with the door.
        if (inTrigger)
        {
            //If inTrigger is true, check for button press to interact with door.
            //For this sample behaviour, we're checking for Fire2, which defaults to the right mouse button.
            if (Input.GetButtonDown("Interact"))
            {
                if (doorOpenClose(inventory))
                    door.doorOpenClose();
            }
        }
    }

   
    public bool doorOpenClose(IInventory inventory)
    {

        if (!doorLocked)
            return true;
        else if (HasRequiredKeycard(inventory.Keycards))
        {
            return true;
        }
        return false;
       
    }


    //Function for unlocking/locking the door, thus enabling/disabling it to be opened and closed.
    //Use toggleDoorLock(true) to lock the door and toggleDoorLock(false) to unlock it.
    public void toggleDoorLock(bool toggleLocked)
    {
            doorLocked = toggleLocked;
            audioChild.GetComponent<AudioSource>().clip = lockingSound;
            audioChild.GetComponent<AudioSource>().Play();
    }


    // Returns bool so can be used in if statement
    public bool HasRequiredKeycard(List<KeyCard> keycards)
    {
        if (keyColour == 0)
        {
            return true;
        }
        for (int i = 0 ; i >= keycards.Count - 1; i++)
        {
            var keycard = keycards[i];

            if (keycard.keycolour == keyColour)
            {
                toggleDoorLock(false);
                return true;
            }
        }
        return false;
    }

}
