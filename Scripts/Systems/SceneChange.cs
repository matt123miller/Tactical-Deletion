using UnityEngine;
using System.Collections;

public class SceneChange : MonoBehaviour
{

    public string sceneName;
    // Use this for initialization
    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            if (Application.loadedLevel == 4)
                ChangeScene("00 MainMenu");
            else
                ChangeScene();
        }
        //ChangeScene(sceneName);
    }

    public void ChangeScene()
    {
        Application.LoadLevel(Application.loadedLevel + 1);
    }

    public void ChangeScene(string name)
    {
        Application.LoadLevel(name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
