/**
 * LoadingScreen.cs
 * Created by: Pedro Borges
 * Created on: 08/05/19 (dd/mm/yy)
 */

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LoadingScreen : MonoBehaviour
{
    Slider slider;
    AsyncOperation async;
    public int sceneIndex;

    public void Initialize()
    {
        slider = GetComponentInChildren<Slider>();
        StartCoroutine(LoadScreen());
    }

    public void InitializeInitial()
    {
        slider = GetComponentInChildren<Slider>();
        StartCoroutine(LoadInitial());
    }

    IEnumerator LoadScreen()
    {
        
        if(SceneManager.GetSceneByBuildIndex(sceneIndex).isLoaded)
            SceneManager.UnloadSceneAsync(sceneIndex);
        yield return new WaitForSeconds(0.2f);
        async = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
        //async.allowSceneActivation = true;
        
        while(async.isDone == false)
        {
            slider.value = async.progress;
            if (async.progress == 0.9f)
            {
                slider.value = 1;
                
            }
            yield return null;
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(sceneIndex));
        gameObject.SetActive(false);
        if(FindObjectOfType<PlayerController>())
            FindObjectOfType<PlayerController>().ReenablePlayer();
        if (FindObjectOfType<MeepController>())
            FindObjectOfType<MeepController>().EnableFollowing();
        slider.value = 0;
        async = null;
    }

    IEnumerator LoadInitial()
    {
        bool a = false;
        async = SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
        async.allowSceneActivation = false;
        while(async.isDone == false)
        {
            slider.value = async.progress;
            if(async.progress == 0.9f)
            {
                slider.value = 1;
                if (!a)
                {
                    async.allowSceneActivation = true;
                    async = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
                    a = true;
                }
                else
                {
                    async.allowSceneActivation = true;
                }                   
            }
            yield return null;
        }
        async = SceneManager.UnloadSceneAsync(0);
        while (async.isDone == false)
        {
            yield return null;
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(2));
        gameObject.SetActive(false);
        slider.value = 0;
        async = null;
    }
}