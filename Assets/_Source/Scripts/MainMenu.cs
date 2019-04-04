/**
 * MainMenu.cs
 * Created by: Pedro Borges
 * Created on: 03/04/19 (dd/mm/yy)
 */

using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void ChangeScene()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}