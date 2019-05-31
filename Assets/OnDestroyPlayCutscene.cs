using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class OnDestroyPlayCutscene : MonoBehaviour
{
    bool done;
    public VideoClip video;
    void OnDestroy()
    {
        if (!video) { Debug.LogWarning("No video assigned to " + gameObject.name); return; }

        if (!done)
        {
            done = true;
            CutsceneManager.instance.SetVideo(video);
            CutsceneManager.instance.videoPlayer.loopPointReached += EndGame;
        }
    }

    void EndGame(VideoPlayer vp)
    {
        SceneManager.LoadScene(0);
    }
}
