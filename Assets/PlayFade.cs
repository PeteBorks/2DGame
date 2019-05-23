using System.Collections;
using UnityEngine;

public class PlayFade : MonoBehaviour
{
    [SerializeField]
    AudioSource audioToPlay;
    float targetVolume;
    float defaultVolume;
    [SerializeField]
    float duration;

    void Awake()
    {
        defaultVolume = audioToPlay.volume;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !audioToPlay.isPlaying)
        {
            targetVolume = defaultVolume;
            audioToPlay.volume = 0;
            audioToPlay.Play();
            StartCoroutine(FadeAudio());
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && audioToPlay.isPlaying)
        {
            targetVolume = 0;
            StartCoroutine(FadeAudio());
        }
    }
    IEnumerator FadeAudio()
    {
        float current = audioToPlay.volume;
        float time = 0;
        while (time < duration*2)
        {
            time += Time.deltaTime;
            audioToPlay.volume = Mathf.Lerp(current, targetVolume, time);
            yield return null;
        }
    }
}
