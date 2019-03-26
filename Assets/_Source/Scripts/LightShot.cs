/**
 * LightShot.cs
 * Created by: Pedro Borges
 * Created on: 26/03/19 (dd/mm/yy)
 */

using UnityEngine;
using System.Collections;

public class LightShot : MonoBehaviour
{
    Light l;
    public float duration = 0.05f;
    public float intensity1 = 0;
    public float intensity2 = 5;
    float t;
    bool once;

    private void Start()
    {
        l = GetComponent<Light>();
        StartCoroutine(OnceDelay());
        l.intensity = intensity2;
    }

    private void Update()
    {
        l.intensity = Mathf.Lerp(l.intensity, 0, duration);
        //if(!once)
        //{ 
        //    t = Mathf.PingPong(Time.time, duration) / duration;
        //    l.intensity = Mathf.Lerp(intensity1, intensity2, t);
        //}
        
    }

    IEnumerator OnceDelay()
    {
        yield return new WaitForSeconds(duration * 2);
        once = true;
        l.intensity = 0;
        t = 0;
    }

    private void OnDisable()
    {
        l.intensity = 0;
    }
}