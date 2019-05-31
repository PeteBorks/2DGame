using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public enum AudioChannel {Master, Sfx, Music};

    float masterVolumePercent = .2f;
    float musicVolumePercent = 1;
    float sfxVolumePercent = 1f;

    AudioSource[] musicSources;
    int activeMusicSourceIndex;

    public static AudioManager instance;

    SoundLibrary library;
    
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            library = GetComponent<SoundLibrary>();
            musicSources = new AudioSource[2];
            for (int i = 0; i < 2; i++)
            {
                GameObject newMusicSource = new GameObject("Music Source " + (i + 1));
                musicSources[i] = newMusicSource.AddComponent<AudioSource>();
                newMusicSource.transform.parent = transform;
            }

            masterVolumePercent = PlayerPrefs.GetFloat("Master vol", masterVolumePercent);
            sfxVolumePercent = PlayerPrefs.GetFloat("Sfx vol", sfxVolumePercent);
            musicVolumePercent = PlayerPrefs.GetFloat("Music vol", musicVolumePercent);
        }
    }

    public void SetVolume(float volumePercent, AudioChannel channel)
    {
        switch (channel)
       {
           case AudioChannel.Master:
                masterVolumePercent = volumePercent;
                break;
            case AudioChannel.Sfx:
               sfxVolumePercent = volumePercent;
                break;
            case AudioChannel.Music:
                musicVolumePercent = volumePercent;
                break;
        }

        musicSources[0].volume = musicVolumePercent * masterVolumePercent;
        musicSources[1].volume = musicVolumePercent * masterVolumePercent;

        PlayerPrefs.SetFloat("Master vol", masterVolumePercent);
        PlayerPrefs.SetFloat("Music vol", musicVolumePercent);
        PlayerPrefs.SetFloat("Sfx vol", sfxVolumePercent);
    }

    public void PlayMusic(AudioClip clip, float fadeDuration = 1)
    {
        activeMusicSourceIndex = 1 - activeMusicSourceIndex;
        if (!clip) return;
        musicSources[activeMusicSourceIndex].clip = clip;
        musicSources[activeMusicSourceIndex].Play();

        StartCoroutine(AnimateMusicCrossfade(fadeDuration));
    }

    public void StopMusic()
    {
        musicSources[activeMusicSourceIndex].Stop();
    }

    public AudioSource PlaySound(AudioClip clip, Vector3 pos, float volume)
    {
        if (clip != null)
        {
            return PlayClipAt(clip, pos, sfxVolumePercent * masterVolumePercent * volume);
        }
        return null;

        
    }
    public AudioSource PlaySound2D(AudioClip clip, Vector3 pos, float volume)
    {
        if (clip != null)
        {
            return PlayClip2D(clip, pos, sfxVolumePercent * masterVolumePercent * volume);
        }
        return null;


    }
    public AudioSource PlaySound(AudioClip clip, Vector3 pos, float volume, float pitch)
    {
        if (clip != null)
        {
            AudioSource c = PlayClipAt(clip, pos, sfxVolumePercent * masterVolumePercent * volume);
            c.pitch = pitch;
            return c;
        }
        return null;


    }

    public void PlaySound (string soundName, Vector3 pos, float volume)
    {
        AudioClip s = library.GetClipFromName(soundName);
        if (!s)
        {
            Debug.LogWarning("Sound " + soundName + " not found on SoundLibrary");
            return;
        }
        else
            PlaySound(s, pos, volume);
    }

    public AudioSource PlaySound(string soundName, Vector3 pos, float volume, float pitch)
    {
        AudioClip s = library.GetClipFromName(soundName);
        if (!s)
        {
            Debug.LogWarning("Sound " + soundName + " not found on SoundLibrary");
            return null;
        }
        else
           return PlaySound(s, pos, volume, pitch);
    }

    public AudioSource PlaySoundLoop(string soundName, Vector3 pos, float volume, float pitch)
    {
        AudioClip clip = library.GetClipFromName(soundName);
        if (clip != null)
        {
            AudioSource c = PlayClipAtLoop(clip, pos, sfxVolumePercent * masterVolumePercent * volume);
            c.pitch = pitch;
            return c;
        }
        return null;
        }


        IEnumerator AnimateMusicCrossfade (float duration)
    {
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * 1 / duration;
            musicSources[activeMusicSourceIndex].volume = Mathf.Lerp(0, musicVolumePercent * masterVolumePercent, percent);
            musicSources[1 - activeMusicSourceIndex].volume = Mathf.Lerp(musicVolumePercent * masterVolumePercent, 0, percent);
            yield return null;
        }
    }
    AudioSource PlayClipAt(AudioClip clip, Vector3 pos, float volume)
    {
        GameObject tempGO = new GameObject("TempAudio"); // create the temp object
        tempGO.transform.position = pos; // set its position
        AudioSource aSource = tempGO.AddComponent<AudioSource>(); // add an audio source
        aSource.clip = clip; // define the clip
        aSource.volume = volume;
        aSource.minDistance = 9;
        aSource.maxDistance = 18f;
        aSource.spatialBlend = 1;
                             // set other aSource properties here, if desired
        aSource.Play(); // start the sound
        Destroy(tempGO, clip.length); // destroy object after clip duration
        return aSource; // return the AudioSource reference
    }

    AudioSource PlayClipAtLoop(AudioClip clip, Vector3 pos, float volume)
    {
        GameObject tempGO = new GameObject("TempAudio"); // create the temp object
        tempGO.transform.position = pos; // set its position
        AudioSource aSource = tempGO.AddComponent<AudioSource>(); // add an audio source
        aSource.clip = clip; // define the clip
        aSource.volume = volume;
        aSource.loop = true;
        aSource.minDistance = 9;
        aSource.maxDistance = 18f;
        aSource.spatialBlend = 1;
        // set other aSource properties here, if desired
        aSource.Play(); // start the sound
        //Destroy(tempGO, clip.length); // destroy object after clip duration
        return aSource; // return the AudioSource reference
    }

    AudioSource PlayClip2D(AudioClip clip, Vector3 pos, float volume)
    {
        GameObject tempGO = new GameObject("TempAudio"); // create the temp object
        tempGO.transform.position = pos; // set its position
        AudioSource aSource = tempGO.AddComponent<AudioSource>(); // add an audio source
        aSource.clip = clip; // define the clip
        aSource.volume = volume;
        aSource.minDistance = 9;
        aSource.maxDistance = 18f;
        aSource.spatialBlend = 0;
        // set other aSource properties here, if desired
        aSource.Play(); // start the sound
        Destroy(tempGO, clip.length); // destroy object after clip duration
        return aSource; // return the AudioSource reference
    }
}
