using UnityEngine;
using System.Collections;

public class GUIMenuBehaviour : MonoBehaviour
{
    private AudioSource[] audios;
    private AudioSource buttonSound, buttonClick;

   

    // Use this for initialization
	void Start ()
	{
	    audios = GetComponents<AudioSource>();
	    buttonSound = audios[0];
	   // buttonClick = audios[1];
	}
	
	// Update is called once per frame
	

    public void ButtonOver()
    {
        buttonSound.Play();
    }


    //public void ButtonClick()
    //{
    //    buttonClick.Play();
    //}
}
