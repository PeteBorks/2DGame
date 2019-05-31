using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class VignetteFeedback : MonoBehaviour
{
    PlayerController player;
    [SerializeField]
    PostProcessVolume postProcessVolume;
    float defaultIntensity;
    [SerializeField]
    float duration = 1;
    [SerializeField]
    AudioClip heartbeat;
    AudioSource source;
    bool playing;

    ColorGrading colorGrading = null;
    Vignette vignette = null;
    void Start()
    {
        player = GetComponent<PlayerController>();
        postProcessVolume.profile.TryGetSettings(out vignette);
        postProcessVolume.profile.TryGetSettings(out colorGrading);
        defaultIntensity = vignette.intensity.value;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.health == 0 && source)
        {
            source.Stop();
        }
        if (player.health <=25)
        {
            float t = Mathf.PingPong(Time.time, duration/3) / duration/3;
            vignette.intensity.value = Mathf.Lerp(0.65f, 0.8f, t);
            vignette.color.value = new Color(1.3f, 0, 0);
            if(!source)
                source = AudioManager.instance.PlaySound2D(heartbeat, transform.position, 0.4f);
        }
        else if (player.health <= 50)
        {
            float t = Mathf.PingPong(Time.time, duration) / duration;
            vignette.intensity.value = Mathf.Lerp(0.3f, 0.6f, t);
            vignette.color.value = new Color(1f, 0, 0);
        }
        else if(player.health>50)
        {
            if (source && source.isPlaying)
            {
                StartCoroutine(FadeAudioOut());
            }
        }
    }

    public void Life()
    {
        vignette.intensity.value = 0.7f;
        vignette.color.value = Color.green;
        colorGrading.colorFilter.value = new Color(0.6f, 1, 0.6f);
        StartCoroutine(FadeVignette());
    }

    public void Hit()
    {
        if (playing || player.health <= 25)
            StopCoroutine(FadeVignette());
        vignette.intensity.value = 0.7f;
        vignette.color.value = Color.red;
        colorGrading.colorFilter.value = new Color(1f, 0.6f, 0.6f);
        StartCoroutine(FadeVignette());
    }
    public IEnumerator FadeVignette()
    {
        playing = true;
        float current = vignette.intensity.value;
        Color currentColor = colorGrading.colorFilter.value;
        float t = 0;
        while(t < duration)
        {
            t += Time.deltaTime;
            vignette.intensity.value = Mathf.Lerp(current, defaultIntensity, t);
            if(colorGrading.colorFilter.value != Color.white)
                colorGrading.colorFilter.value = Color.Lerp(currentColor, Color.white, t);
            yield return null;
        }
        vignette.color.value = Color.black;
        playing = false;
    }
    IEnumerator FadeAudioOut()
    {
        float current = source.volume;
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            source.volume = Mathf.Lerp(current, 0, time);
            yield return null;
        }
        source = null;
    }
}
