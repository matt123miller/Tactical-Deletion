using UnityEngine;
using System.Collections;

public class load : MonoBehaviour {

    public float fadeSpeed = 1.5f;            // How fast the light fades between intensities.
    public float highIntensity = 0.8f;        // The maximum intensity of the light whilst the alarm is on.
    public float lowIntensity = 0.5f;       // The minimum intensity of the light whilst the alarm is on.
    public float changeMargin = 0.2f;       // The margin within which the target intensity is changed.
    public static bool alarmState;
    private float timer;
    private SceneChange sc;


    void Awake()
    {
        sc = FindObjectOfType<SceneChange>();
        // When level starts, light "off".
        GetComponent<Light>().intensity = 0f;

        // When the alarm starts for the first time, the light should aim to have the maximum intensity.
        targetIntensity = highIntensity;
    }


    public static float targetIntensity; 

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 3f)
            sc.ChangeScene();
            
            // ... Lerp the light's intensity towards the current target.
            GetComponent<Light>().intensity = Mathf.Lerp(GetComponent<Light>().intensity, targetIntensity, fadeSpeed * Time.deltaTime);

            // Check whether the target intensity needs changing and change it if so.
            CheckTargetIntensity();
        
    }


    void CheckTargetIntensity()
    {
        // If the difference between the target and current intensities is less than the change margin...
        if (Mathf.Abs(targetIntensity - GetComponent<Light>().intensity) < changeMargin)
        {
            // ... if the target intensity is high...
            if (targetIntensity == highIntensity)
                // ... then set the target to low.
                targetIntensity = lowIntensity;
            else
                // Otherwise set the targer to high.
                targetIntensity = highIntensity;
        }
    }
}

