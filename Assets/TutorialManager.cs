using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Canvas canvas;
    public TextMeshProUGUI text;
    public RawImage rawImage;

    public static TutorialManager instance;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if(videoPlayer.isPlaying && Input.GetButtonDown("Interact"))
        {
            StopVideo();
        }
    }

    public void SetVideo(VideoClip v)
    {
        videoPlayer.clip = v;
        StartCoroutine(PrepareVideo());
    }

    public void SetText(string s)
    {
        text.text = s;
    }

    public void RunVideo()
    {
        if(videoPlayer.clip)
        {
            canvas.gameObject.SetActive(true);
            videoPlayer.Play();
            Time.timeScale = 0;
            FindObjectOfType<Main>().DisableInput();
        }
    }

    public void StopVideo()
    {
        if(videoPlayer.isPlaying)
        {
            videoPlayer.Stop();
            Time.timeScale = 1;
            canvas.gameObject.SetActive(false);
            FindObjectOfType<Main>().EnableInput();
        }
    }

    IEnumerator PrepareVideo()
    {
        videoPlayer.Prepare();
        while(!videoPlayer.isPrepared)
        {
            yield return null;
        }
        rawImage.texture = videoPlayer.texture;
        RunVideo();
    }
}
