using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;

public class Tutorial : MonoBehaviour
{
    public VideoClip video;
    public string text;
    bool done;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!video) { Debug.LogWarning("No video assigned to " + gameObject.name); return; }
        if (text == string.Empty) { Debug.LogWarning("No text assigned to " + gameObject.name); return; }

        if(collision.GetComponent<PlayerController>() && !done)
        {
            done = true;
            TutorialManager.instance.SetVideo(video);
            TutorialManager.instance.SetText(text);
        }
    }
}
