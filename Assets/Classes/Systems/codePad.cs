using System;
using UnityEngine;
using System.Collections;

public class codePad : MonoBehaviour {
    
    public string targetPassword = "";
    public string inputPassword = "";
    private bool padActive = true;
	
    // Use this for initialization
	void Start () {
	
	}

    void OnGUI()
    {
        if (padActive == true)
        {
            Time.timeScale = 0.0f;

            GUI.BeginGroup(new Rect(Screen.width / 2 - 110, Screen.height / 2 - 110, 210, 270));

            //Use a GUI skin to make it pretty
            
            GUI.Box(new Rect(0, 0, 210, 270), "");
            if (GUI.Button(new Rect(15, 80, 50, 50), "1"))      inputPassword += "1   ";
            if (GUI.Button(new Rect(80, 80, 50, 50), "2"))      inputPassword += "2   ";
            if (GUI.Button(new Rect(145, 80, 50, 50), "3"))     inputPassword += "3   ";
            if (GUI.Button(new Rect(15, 140, 50, 50), "4"))     inputPassword += "4   ";
            if (GUI.Button(new Rect(80, 140, 50, 50), "5"))     inputPassword += "5   ";
            if (GUI.Button(new Rect(145, 140, 50, 50), "6"))    inputPassword += "6   ";
            if (GUI.Button(new Rect(15, 200, 50, 50), "7"))     inputPassword += "7   ";
            if (GUI.Button(new Rect(80, 200, 50, 50), "8"))     inputPassword += "8   ";
            if (GUI.Button(new Rect(145, 200, 50, 50), "9"))    inputPassword += "9   ";
            GUI.Box(new Rect(15, 20, 180, 50), "");
            GUI.Label(new Rect(75, 35, 150, 50), inputPassword);
        
            GUI.EndGroup();
        }
    }


	// Update is called once per frame
	void Update () {
	    if (inputPassword == targetPassword)
	    {
            Debug.Log("success!");
	    }
	}
}
