using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ClickToLoadAsync : MonoBehaviour
{

    public Slider loadingBar;
    public GameObject loadingImage;
    public bool loadOnStart;

    private AsyncOperation async;

    void Start()
    {
        if (loadOnStart)
        {
            loadingImage.SetActive(true);
            StartCoroutine(LoadNextLevelWithBar());
            return;
        }
    }


    public void ClickAsync(int level)
    {
        loadingImage.SetActive(true);
        //StartCoroutine(LoadLevelWithBar(level));
        StartCoroutine(LoadNextLevelWithBar());
    }

    //load next level
    IEnumerator LoadNextLevelWithBar()
    {
        async = Application.LoadLevelAsync(Application.loadedLevel + 1);
        while (!async.isDone)
        {
            loadingBar.value = async.progress;
            yield return null;
        }
    }

    //load specific level
    IEnumerator LoadSpecificLevelWithBar(int level)
    {
        async = Application.LoadLevelAsync(level);
        while (!async.isDone)
        {
            loadingBar.value = async.progress;
            yield return null;
        }
    }
}