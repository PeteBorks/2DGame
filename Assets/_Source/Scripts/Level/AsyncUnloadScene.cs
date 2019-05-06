/**
 * AsyncUnloadScene.cs
 * Created by: Pedro Borges
 * Created on: 17/04/19 (dd/mm/yy)
 */

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AsyncUnloadScene : MonoBehaviour
{
    [SerializeField]
    int sceneIndexToUnload;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (SceneManager.GetSceneByBuildIndex(sceneIndexToUnload).isLoaded && collision.GetComponent<PlayerController>()) 
            StartCoroutine(UnloadScene());
    }

    IEnumerator UnloadScene()
    {
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneIndexToUnload);
        while (!asyncUnload.isDone)
        {
            yield return null;
        }
            Resources.UnloadUnusedAssets();
    }

}