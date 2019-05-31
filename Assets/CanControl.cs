using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanControl : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
            collision.GetComponent<PlayerController>().canControlStellar = true;
    }
}
