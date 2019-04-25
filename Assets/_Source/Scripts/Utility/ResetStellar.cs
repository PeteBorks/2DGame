/**
 * ResetStellar.cs
 * Created by: Pedro Borges
 * Created on: 24/04/19 (dd/mm/yy)
 */

using UnityEngine;

public class ResetStellar : MonoBehaviour
{
    GetPlayerRef mainRef;
    private void Start()
    {
        mainRef = FindObjectOfType<GetPlayerRef>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerController>())
        {
            mainRef.stellar.reset = true;
        }
    }
}