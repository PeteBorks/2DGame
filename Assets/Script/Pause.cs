using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{

    public GameObject pauseMenuHolder;
    public GameObject optionsMenuHolder;
    public GameObject controlMenuHolder;
    public Transform canvas;

    public Slider[] volumeSlider;

    public void Quit()
    {
        Application.Quit();
    }

    public void OptionsMenu()
    {
        pauseMenuHolder.SetActive(false);
        optionsMenuHolder.SetActive(true);
        controlMenuHolder.SetActive(false);
    }

    public void PauseMenu()
    {
        pauseMenuHolder.SetActive(true);
        optionsMenuHolder.SetActive(false);
        controlMenuHolder.SetActive(false);
    }

    public void ControlMenu()
    {
        pauseMenuHolder.SetActive(false);
        optionsMenuHolder.SetActive(false);
        controlMenuHolder.SetActive(true);
    }

    public void SetMasterVolume (float value)
    {
        
    }

    public void SetMusicVolume(float value)
    {
        
    }

    public void SetSfxVolume(float value)
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Resume();
        }
    }

    public void Resume()
    {
        if (canvas.gameObject.activeInHierarchy == false)
        {
            canvas.gameObject.SetActive(true);
            FindObjectOfType<Main>().playerPawn.inputEnabled = false;
            Time.timeScale = 0;
        }
        else
        {
            canvas.gameObject.SetActive(false);
            FindObjectOfType<Main>().playerPawn.inputEnabled = true;
            Time.timeScale = 1;
        }
    }
}


//if (controlMenuHolder.gameObject.activeInHierarchy == false)
       // {
  //          controlMenuHolder.gameObject.SetActive(true);
            
    //    }
      //  else
        //{
          //  controlMenuHolder.gameObject.SetActive(false);
            
        //}