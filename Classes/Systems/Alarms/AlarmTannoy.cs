using UnityEngine;
using System.Collections;

public class AlarmTannoy : IObserver
{

    private GlobalStateManager gsm;
    private AudioSource audio;
    public AudioClip alarmTrack, allClear;
    private bool alarm;
    // Use this for initialization

    void Start()
    {

        gsm = GlobalStateManager.GSMInstance;
        gsm.Subscribe(this);

        audio = gameObject.GetComponent<AudioSource>();
    }

    override public void UpdateThisObserver(WorldState newState)
    {
        Debug.Log("Getting updated");
        // if (newState == WorldState.alarmState)
        alarm = !alarm;

        if (alarm)
        {

            audio.clip = alarmTrack;
            audio.loop = true;
            audio.Play();
        }
        else
        {

            audio.clip = allClear;
            audio.loop = false;
            audio.Play();
        }
    }
    // Update is called once per frame
    void Update()
    {

    }

}

