using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateRage : MonoBehaviour
{
    [SerializeField]
    Gradient color;
    [SerializeField]
    GameObject weapon1;
    [SerializeField]
    GameObject weapon2;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerController>())
        {
            FindObjectOfType<MeepController>().EnableRage();
            weapon1.SetActive(true);
            weapon2.SetActive(true);
            collision.GetComponentInChildren<TrailRenderer>().colorGradient = color;
        }
    }
}
