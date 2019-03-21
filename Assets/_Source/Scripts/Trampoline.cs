/**
 * Trampoline.cs
 * Created by: Pedro Borges
 * Created on: 20/03/19 (dd/mm/yy)
 */

using UnityEngine;

public class Trampoline : MonoBehaviour
{

    public float force = 2000f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            // Calculate Angle Between the collision point and the player
            Vector2 dir = collision.contacts[0].point - new Vector2(transform.position.x, transform.position.y);
            // We then get the opposite (-Vector3) and normalize it
            dir = -dir.normalized;
            // And finally we add force in the direction of dir and multiply it by force. 
            // This will push back the player
            collision.gameObject.GetComponent<PlayerController>().rb2D.velocity = new Vector2(0,force);
        }
    }
}