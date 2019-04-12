/**
 * MainMenu.cs
 * Created by: Pedro Borges
 * Created on: 03/04/19 (dd/mm/yy)
 */

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void ChangeScene()
    {
    	
    	StartCoroutine(LoadScene(2));
    	StartCoroutine(LoadScene(1));
    	
    }

    IEnumerator LoadScene(int index)
	{
		
	    AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);

	    // Wait until the asynchronous scene fully loads
	    while (!asyncLoad.isDone)
	    {
	        // You can build a slider also, with progress
	        Debug.Log(asyncLoad.progress);
	        yield return null;
	    }
	    SceneManager.UnloadSceneAsync(0);
	}

    public void QuitGame()
    {
        Application.Quit();
    }
}