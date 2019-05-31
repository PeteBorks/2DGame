using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disable : MonoBehaviour
{
    public GameObject gobject;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            gobject.SetActive(false);
            FindObjectOfType<MeepController>().DisableFollowing();
            FindObjectOfType<MeepController>().EnableFollowing();
        }
            
    }
}

