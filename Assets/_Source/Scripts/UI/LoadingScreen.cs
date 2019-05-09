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

    IEnumerator LoadScreen()
    {
        if (SceneManager.GetSceneByBuildIndex(sceneIndex).isLoaded)
        {
            sceneIndex = sceneIndex;
        }
        else if (SceneManager.GetSceneByBuildIndex(sceneIndex - 1).isLoaded && sceneIndex - 1 != SceneManager.GetSceneByName("Main").buildIndex)
            sceneIndex -= 1;
        else
        {
            Debug.LogWarning("No scene to unload");
            yield return null;
        }
        SceneManager.UnloadSceneAsync(sceneIndex);
        yield return new WaitForSeconds(0.2f);
        async = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
        async.allowSceneActivation = false;
        while(async.isDone == false)
        {
            slider.value = async.progress;
            if (async.progress == 0.9f)
            {
                slider.value = 1;
                async.allowSceneActivation = true;
            }
            yield return null;
        }
        gameObject.SetActive(false);
        FindObjectOfType<PlayerController>().ReenablePlayer();
        slider.value = 0;
        async = null;
    }
}