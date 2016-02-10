using System.Collections.Generic;
using ProBuilder2.Common;
using UnityEngine;
using System.Collections;
using System;

 public enum WorldState
    {
        unsureState,
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
    public static GlobalStateManager GSMInstance
    {
        get
        {
            if (gsmInstance == null)
            {
                gsmInstance = new GlobalStateManager();
            }
            return gsmInstance;
        }
    }

    private WorldState gsmState;
    private Vector3 globalLKP;
    private Vector3 resetPosition = new Vector3(1000.0f, 1000.0f, 1000.0f);

    public float timeSinceCamSeen;
    public float timerCheck;
    private bool alarmTimerActive = false;
    private float spottedResetTimer, spottedResetTarget;
    private bool inCameraView;

    // Need to move to same audio changing system as doors.
    private AudioSource audio, spottedSound;
    public AudioClip ambientTrack, spottedTrack;

    private bool paused;
    //public IObserver[] observers;
    public List<IObserver> observers = new List<IObserver>();


    // Properties
    #region

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
        gsmState = WorldState.ambientState;
        observers.Add(this);
        Debug.Log(observers[0]);
    }

    public GlobalStateManager()
    {

    }

    // Use this for initialization
    void Start()
    {
        var audios = gameObject.GetComponents<AudioSource>();
        audio = audios[0];
        
        audio.clip = ambientTrack;
        audio.Play();

        spottedSound = audios[1];
        //pauseMenu.SetActive(false);
    }

    public void Subscribe(IObserver observer) {
        
            observers.Add(observer);
    }

    public void Unsubscribe(IObserver observer) {
        
        observers.Remove(observer);
    }
    
    public void Notify(WorldState newState) {
        
       foreach (var observer in observers) {
           
           observer.UpdateThisObserver(newState);
           
           if (observer is AIManager){
              
               AIManager aim = observer as AIManager;
               aim.LKP = globalLKP;
           }
       }
    }
    
    override public void UpdateThisObserver(WorldState newState) {

        gsmState = newState;
    }

    
    public void UpdateManagers(Vector3 v3) {
      
      
    }
    
    public void SetStateAmbient(){
        
        gsmState = WorldState.ambientState;
        audio.clip = ambientTrack;
           
        if (!audio.isPlaying) {
            
             audio.Play();
        }
    }
    
    public void SetStateAlarm(){
        
        gsmState = WorldState.alarmState;
        
    }
    
    public void SetStateSpotted()
    {
        if (gsmState != WorldState.spottedState)
        {
            gsmState = WorldState.spottedState;
            
            spottedSound.Play();
        }
        //every time the player is spotted the timer is set to 0
        spottedResetTimer = 0f;

    }
    
    public void SetStateUnsure() {
        
        SetStateAmbient();
        ResetGlobalLKP();
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
        Notify(gsmState);
        SetStateSpotted();
    }


    // Find all usages and change them to Notify(). Make AIM implement ObserverUpdate() and get the resetpost property
   
    // Update is called once per frame
    private void Update()
    {
        //if (Input.GetButtonUp("Cancel"))//escape key
        //    TogglePause();

        //alarm state music
        if (gsmState == WorldState.alarmState)
        {
            //spottedState = true;
            //ambientState = false;

            if (AlarmTimerActive)
                timeSinceCamSeen += Time.deltaTime;
            else
                timeSinceCamSeen = 0f;

            if ((timeSinceCamSeen > timerCheck) && (!InAlarmView))
            {
                SetStateSpotted();
                timeSinceCamSeen = 0f;
            }
        }
        else if (gsmState == WorldState.spottedState)
        {
            spottedResetTimer += Time.deltaTime;

            if (spottedResetTimer > spottedResetTarget)
            {
                Debug.Log("Spotted timer reset.");
                //do we now change out of spottedState?
                spottedResetTimer = 0f;
            }
        }
        else if (gsmState == WorldState.ambientState)
        {
            
        }
        else if (gsmState == WorldState.unsureState)
        {
            SetStateAmbient();
            ResetGlobalLKP();
            Notify(gsmState);
        }
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
