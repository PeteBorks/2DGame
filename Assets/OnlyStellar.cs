using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyStellar : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        MeepController stellar = collision.GetComponent<MeepController>();
        if(stellar)
        {
            stellar.canChangePawn = false;
            stellar.canPlatform = false;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        MeepController stellar = collision.GetComponent<MeepController>();
        if (stellar)
        {
            stellar.canChangePawn = true;
            stellar.canPlatform = true;
        }
    }
}
