using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Cutscene : MonoBehaviour
{
    bool done;
    public VideoClip video;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!video) { Debug.LogWarning("No video assigned to " + gameObject.name); return; }

        if (collision.GetComponent<PlayerController>() && !done)
        {
            done = true;
            CutsceneManager.instance.SetVideo(video);
        }
    }

    
}
