using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public AudioClip mainTheme;
    public AudioClip menuTheme;

    private void Start()
    {
        AudioManager.instance.PlayMusic(menuTheme, 2);
    }

    private void Update()
    {
        if(Input.GetButtonDown("Jump"))
        {
            AudioManager.instance.PlayMusic(mainTheme, 3);
        }
    }
}
