using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXEmitter : MonoBehaviour
{
    [SerializeField]
    float volume = 0.25f;
    [SerializeField]
    float pitch = 0.85f;
    public void PlayFootstep()
    {
        AudioManager.instance.PlaySound("footstep", transform.position, volume, pitch);
    }

    public void PlayCrack()
    {
            AudioManager.instance.PlaySound("crack", transform.position, 0.3f, 1);
    }
}
