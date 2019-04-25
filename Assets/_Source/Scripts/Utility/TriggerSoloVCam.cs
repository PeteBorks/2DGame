/**
 * TriggerSoloVCam.cs
 * Created by: Pedro Borges
 * Created on: 24/04/19 (dd/mm/yy)
 */

using UnityEngine;
using Cinemachine;

public class TriggerSoloVCam : MonoBehaviour
{
    public GameObject VCam;
    GetPlayerRef refs;

    void Start()
    {
        refs = FindObjectOfType<GetPlayerRef>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
            VCam.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
            VCam.SetActive(false);
    }
}