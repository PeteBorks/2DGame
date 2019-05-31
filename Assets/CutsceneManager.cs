using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    public Canvas canvas;
    public VideoPlayer videoPlayer;
    public RawImage rawImage;
    public AudioSource audioSource;
    public static CutsceneManager instance;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (videoPlayer.isPlaying && Input.GetButtonDown("Interact"))
        {
            StopVideo(videoPlayer);
        }
    }
    public void SetVideo(VideoClip v)
    {
        videoPlayer.clip = v;
        StartCoroutine(PrepareVideo());
    }

    public void RunVideo()
    {
        if (videoPlayer.clip)
        {
            canvas.gameObject.SetActive(true);
            videoPlayer.Play();
            audioSource.Play();
            Time.timeScale = 0;
            FindObjectOfType<Main>().DisableInput();
            videoPlayer.loopPointReached += StopVideo;
        }
    }

    public void StopVideo(VideoPlayer vp)
    {
        videoPlayer.Stop();
        audioSource.Stop();
        Time.timeScale = 1;
        canvas.gameObject.SetActive(false);
        FindObjectOfType<Main>().EnableInput();
    }

    IEnumerator PrepareVideo()
    {
        videoPlayer.Prepare();
        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }
        rawImage.texture = videoPlayer.texture;
        RunVideo();
    }

}
