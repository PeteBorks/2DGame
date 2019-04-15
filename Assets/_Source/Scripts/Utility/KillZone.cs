/**
 * KillZone.cs
 * Created by: Pedro Borges
 * Created on: 12/04/19 (dd/mm/yy)
 */

using UnityEngine;

public class KillZone : MonoBehaviour
{
    public Vector2 returnPos;
    void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player)
            player.transform.position = returnPos;
    }
}