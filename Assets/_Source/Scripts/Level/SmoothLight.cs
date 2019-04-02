/**
 * SmoothLight.cs
 * Created by: Pedro Borges
 * Created on: 25/03/19 (dd/mm/yy)
 */

using UnityEngine;

public class SmoothLight : MonoBehaviour
{
    Light l;
    public float duration = 1.0f;
    public float intensity1 = 0;
    public float intensity2 = 5;

    private void Start()
    {
        l = GetComponent<Light>();
    }

    private void Update()
    {
        float t = Mathf.PingPong(Time.time, duration) / duration;
        l.intensity = Mathf.Lerp(0, 5, t);
    }

    private void OnDisable()
    {
        l.intensity = 0;
    }
}