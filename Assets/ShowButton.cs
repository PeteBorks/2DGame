using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowButton : MonoBehaviour
{
    public GameObject indicator;
    bool canVisible = true;
    bool canInteract;
    GameObject player;


    void Update()
    {
        if(canInteract && Input.GetButtonDown("Interact"))
        {
            canInteract = false;
            canVisible = false;
            indicator.SetActive(false);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
            player = collision.GetComponent<PlayerController>().gameObject;
        if (player && canVisible)
        {
            indicator.SetActive(true);
            canInteract = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(player)
        {
            indicator.SetActive(false);
            canInteract = false;
        }
    }
}
