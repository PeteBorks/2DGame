/**
 * AsyncLoadScene.cs
 * Created by: Pedro Borges
 * Created on: 11/04/19 (dd/mm/yy)
 */

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class AsyncLoadScene : MonoBehaviour
{
	[SerializeField]
	int sceneIndexToLoad;

	void OnTriggerEnter2D(Collider2D collision)
	{
		StartCoroutine(LoadScene());
	}

	IEnumerator LoadScene()
	{
	    AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndexToLoad, LoadSceneMode.Additive);

	    // Wait until the asynchronous scene fully loads
	    while (!asyncLoad.isDone)
	    {
	        // You can build a slider also, with progress
	        Debug.Log(asyncLoad.progress);
	        yield return null;
	    }
	}
}