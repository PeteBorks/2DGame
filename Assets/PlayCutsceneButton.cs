using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PlayCutsceneButton : MonoBehaviour
{
    public VideoClip video;
    public GameObject sprite;
    GameObject player;
    bool canPlay;
    bool played;

    void Update()
    {
        if(canPlay && !played && Input.GetButtonDown("Interact"))
        {
            CutsceneManager.instance.SetVideo(video);
            played = true;
            sprite.SetActive(false);
            canPlay = false;
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.GetComponent<PlayerController>())
            player = collision.GetComponent<PlayerController>().gameObject;
        if (player && !played)
        {
            sprite.SetActive(true);
            canPlay = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (player)
        {
            sprite.SetActive(false);
            canPlay = false;
        }
    }
}
