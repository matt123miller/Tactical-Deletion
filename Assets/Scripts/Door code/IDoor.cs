using UnityEngine;
using System.Collections;


public abstract class IDoor : MonoBehaviour {

    [Header("Parent IDoor variables")]
    public GameObject doorChild;
    public AudioSource audioChild;

    public AudioClip openSound;
    public AudioClip closeSound;

    public bool doorOpen = false; 

    public abstract void doorOpenClose();

    // If overriden make sure to call GetDoorRequirements();
    protected virtual void Start()
    {

        audioChild = GetComponentInChildren<AudioSource>();
        GetDoorRequirements();
    }


    public virtual void GetDoorRequirements()
    {

        doorChild = transform.GetChild(0).gameObject;

    }
}
