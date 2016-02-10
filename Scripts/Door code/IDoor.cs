using UnityEngine;
using System.Collections;


public abstract class IDoor : MonoBehaviour {

    [Header("Parent IDoor variables")]
    public GameObject doorChild;
    public GameObject audioChild;

    public AudioClip openSound;
    public AudioClip closeSound;

    public bool doorOpen = false; 

    public abstract void doorOpenClose();
}
