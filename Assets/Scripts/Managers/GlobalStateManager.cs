using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;

 public enum WorldState
    {
        ambientState,
        alarmState,
        spottedState
    }

[RequireComponent(typeof(AudioReverbZone))]
[RequireComponent(typeof(AudioLowPassFilter))]
public  class GlobalStateManager : IObserver, ISubject
{
    
    private static GlobalStateManager gsmInstance;

    // Singleton pattern
    // Access this in Start, not Awake so that it has time to be instantiated.
    public static GlobalStateManager GSMInstance
    {
        get
        {
            return gsmInstance;
        }
    }

    private WorldState gsmState;
    private Vector3 globalLKP;
    private Vector3 resetPosition = new Vector3(1000.0f, 1000.0f, 1000.0f);

    private float spottedResetTimer, timeSinceCamSeen;
    public float TimeSinceCamSeenTarget, spottedResetTarget;
    private bool alarmTimerActive = false;
    private bool inCameraView;

    // Need to move to same audio changing system as doors.
    private AudioSource audioTrack, spottedSound;
    public AudioClip ambientTrack, spottedTrack;

    private bool paused;

    public List<IObserver> observers = new List<IObserver>();


    
    #region Properties

    public WorldState GSMState
    {
        get { return gsmState; }
        set { gsmState = value; }
    }

    public Vector3 GlobalLkP
    {
        get { return globalLKP; }
        set { globalLKP = value; }
    }

    public Vector3 ResetPosition
    {
        get { return resetPosition; }
        set { resetPosition = value; }
    }
    
    public float TimeSinceCamSeen
    {
        get { return timeSinceCamSeen; }
        set { timeSinceCamSeen = value; }
    }

    public bool InAlarmView
    {
        get { return inCameraView; }
        set { inCameraView = value; }
    }

    public bool AlarmTimerActive
    {
        get { return alarmTimerActive; }
        set { alarmTimerActive = value; }
    }

    public bool Paused
    {
        get { return paused; }
        set { paused = value; }
    }

    #endregion // Properties

    private void Awake()
    {
        gsmInstance = this;

        gsmState = WorldState.ambientState;
        //observers.Add(this);
    }

    public GlobalStateManager()
    {

    }

    // Use this for initialization
    void Start()
    {
        var audios = gameObject.GetComponents<AudioSource>();
        audioTrack = audios[0];
        
        audioTrack.clip = ambientTrack;
        audioTrack.Play();

        spottedSound = audios[1];

        SetStateAmbient();
        ResetGlobalLKP();
        //pauseMenu.SetActive(false);
    }

    public void Subscribe(IObserver observer) {
        
            observers.Add(observer);
        Debug.Log(observer);
    }

    public void Unsubscribe(IObserver observer) {
        
        observers.Remove(observer);
    }
    
    public void Notify(WorldState newState) {
        
       foreach (var observer in observers) {
           
           observer.UpdateThisObserver(newState);
           
           //if (observer is AIManager){
              
           //    AIManager aim = observer as AIManager;
           //    aim.LKP = globalLKP;
           //}
       }
    }
    
    override public void UpdateThisObserver(WorldState newState) {

        gsmState = newState;
        switch (newState) {

            case WorldState.alarmState:
                SetStateAlarm();
                break;

            case WorldState.ambientState:
                SetStateAmbient();
                break;

            case WorldState.spottedState:
                SetStateSpotted();
                break;

            default:
                break;
        }
    }

    
    public void UpdateManagers(Vector3 v3) {
      
      // replaced by observer pattern
    }
    
    public void SetStateAmbient(){
        
        gsmState = WorldState.ambientState;
        ResetGlobalLKP();

        audioTrack.clip = ambientTrack;
          
        // Is this IF necessary? Surely you can just play audio anyway with new track. 
        if (!audioTrack.isPlaying) {
            
             audioTrack.Play();
        }

        Notify(gsmState);
    }
    
    public void SetStateAlarm(){
        
        gsmState = WorldState.alarmState;

        // Play alarm music

        Notify(gsmState);
    }
    
    public void SetStateSpotted()
    {

        if (gsmState != WorldState.spottedState)
        {
            Debug.Log("Spotted!");
            gsmState = WorldState.spottedState;
            
            spottedSound.Play();
        }
        //every time the player is spotted the timer is set to 0
        spottedResetTimer = 0f;

        Notify(gsmState);
    }
    
    
    public void ResetGlobalLKP()
    {
        globalLKP = resetPosition;
    }


    public void StartAlarmTimer()
    {
        InAlarmView = false;
        AlarmTimerActive = true;
    }

    

    public void CamSpotted(Vector3 v3)
    {
        InAlarmView = true;
        AlarmTimerActive = false;
        globalLKP = v3;
        SetStateSpotted();
    }


    // Find all usages and change them to Notify(). Make AIM implement ObserverUpdate() and get the resetpost property
   
    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonUp("Cancel"))//escape key
            TogglePause();

        switch(gsmState)
        {
            case WorldState.alarmState:

                if (AlarmTimerActive && !InAlarmView)
                {
                    if ((timeSinceCamSeen += Time.deltaTime) > TimeSinceCamSeenTarget)
                    {
                        SetStateSpotted();
                        timeSinceCamSeen = 0f;
                    }
                }
                else
                    timeSinceCamSeen = 0f;

                break;

            case WorldState.spottedState:
                
                if ((spottedResetTimer  += Time.deltaTime)> spottedResetTarget)
                {
                    Debug.Log("Spotted timer reset.");
                    //do we now change out of spottedState?
                    spottedResetTimer = 0f;
                }
                break;

            case WorldState.ambientState:

                
                break;
                
            default:

                break;
        }
    }

    private void TogglePause()
    {
        Time.timeScale = 0f;

        // Eventually turn some GUI on
    }

    // unused
    private IEnumerator FadeAudioInOut(AudioSource reduce, AudioSource increase)
    {
        int duration = 3;

        float startTime = Time.time;
        float endTime = startTime + duration;

        while (Time.time > endTime)
        {

            var i = (Time.time - startTime) / duration;

            reduce.volume = (1 - i);
            increase.volume = i;


            yield return null;

        }
    }
}
