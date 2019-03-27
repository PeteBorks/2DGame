/**
 * FlickeringLight.cs
 * Created by: Pedro Borges
 * Created on: 26/03/19 (dd/mm/yy)
 */

using UnityEngine;
using System.Collections;

public class FlickeringLight : MonoBehaviour
{
    Light testLight;
    public bool random;
    public float waitTime;
    public float minWaitTime;
    public float maxWaitTime;

    void Start()
    {
        testLight = GetComponent<Light>();
        if (random)
            StartCoroutine(FlashingRandom());
        else
            StartCoroutine(Flashing());
    }

    IEnumerator FlashingRandom()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
            testLight.enabled = !testLight.enabled;
        }
    }

    IEnumerator Flashing()
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            testLight.enabled = !testLight.enabled;
        }
    }
}