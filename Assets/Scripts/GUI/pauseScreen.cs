using UnityEngine;
using System.Collections;

public class pauseScreen : MonoBehaviour
{
    bool paused = false;
    private bool menuBool = false;
    private bool quitBool = false;
    private GlobalStateManager gsm;

    private void Start()
    {
        menuBool = false;
        quitBool = false;
        gsm = FindObjectOfType<GlobalStateManager>();
    }

    void Update()
     {
        if (Input.GetButtonDown("Cancel"))
        {
            paused = togglePause();
            
        }
     }
     
     void OnGUI()
     {
         //GUI.BegiGroup() at the beginning, GUI.EndGroup() at the end, similiar to HTML <>
             //defines the position of a new GUI group
             //for all GUI items inside the group, 0, 0 is now defined from the BeginGroup() position
         GUI.BeginGroup(new Rect(Screen.width/2 - 150, Screen.height/2 - 150, 300, 300));
         
         if (paused == true && menuBool == false && quitBool == false)
         {
             GUI.Box(new Rect(0, 0, 300, 300), "PAUSE");

             if (GUI.Button(new Rect(35, 40, 230, 30), "Resume")) paused = togglePause();
             if (GUI.Button(new Rect(35, 80, 230, 30), "Quit to Menu")) menuBool = !menuBool;
             if (GUI.Button(new Rect(35, 120, 230, 30), "Quit to Windows")) quitBool = !quitBool;
         }
         //try to open new gui window through bool flag
             if(menuBool)
             {
                 GUI.Box(new Rect(0,0,300,300), "Are you sure?");
                 GUI.Label(new Rect(35, 35, 230, 50), "Any unsaved progress will be lost.");
                 if (GUI.Button(new Rect(35, 100, 230, 30), "Yes")) Application.LoadLevel(0);
                 if (GUI.Button(new Rect(35, 150, 230, 30), "No")) menuBool = !menuBool;
             }

             if(quitBool)
             {
                 GUI.Box(new Rect(0, 0, 300, 300), "Are you sure?");
                 GUI.Label(new Rect(35, 35, 230, 50), "Any unsaved progress will be lost.");
                 if (GUI.Button(new Rect(35, 100, 230, 40), "Yes")) Application.Quit();
                 if (GUI.Button(new Rect(35, 150, 230, 40), "No")) quitBool = !quitBool;
             }

            GUI.EndGroup();
            
         }
     
     
     bool togglePause()
     {
         if(Time.timeScale == 0f)
         {
             Time.timeScale = 1f;
             gsm.Paused = false;
             return(false);
             
         }
         else
         { 
             Time.timeScale = 0f;
             gsm.Paused = true;
             return(true);    
         }
     }
 }