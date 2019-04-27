/**
 * TriggerCloseCam.cs
 * Created by: Pedro Borges
 * Created on: 26/04/19 (dd/mm/yy)
 */

using UnityEngine;

public class TriggerCloseCam : MonoBehaviour
{
    public float slowSpeed;
    GetPlayerRef refs;
    MeepController stellar;
    float defaultSpeed;
    void Start()
    {
        refs = FindObjectOfType<GetPlayerRef>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (refs)
            stellar = refs.stellar.GetComponent<MeepController>();
        if (stellar)
        {
            defaultSpeed = stellar.speed;
            stellar.ChangeCam();
            stellar.speed = slowSpeed;
        }
            
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (refs)
            stellar = refs.stellar.GetComponent<MeepController>();
        if (stellar)
        {
            stellar.ChangeCam();
            stellar.speed = defaultSpeed;
        }
            
    }
}