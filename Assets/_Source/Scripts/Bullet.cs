/**
 * Shot.cs
 * Created by: Pedro Borges
 * Created on: 26/03/19 (dd/mm/yy)
 */

using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20;
    public float damage = 20;
    public GameObject hitFX;
    [HideInInspector]
    public bool isRight = false;
    Collider2D collider2;
    Rigidbody2D rb2d;
    GameObject hit;

    private void Start()
    {
        collider2 = GetComponent<CapsuleCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        if (!isRight)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            speed = -speed;
        }
        rb2d.velocity = new Vector2(speed, 0);
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.GetComponent<Bullet>())
        {
            hit = Instantiate(hitFX, transform.position, Quaternion.identity);
            if (!isRight)
            {
                hit.transform.localScale = new Vector3(-1, 1, 1);
                hit.transform.position -= new Vector3(-0.2f, 0, 0);
            }
            else
            {
                hit.transform.localScale = new Vector3(1, 1, 1);
                hit.transform.position -= new Vector3(0.2f, 0, 0);
            }
            if (collision.gameObject.GetComponent<ButtonEnable>())
            {
                collision.gameObject.GetComponent<ButtonEnable>().OnInteract();
            }
            if(collision.GetComponent<BaseEntity>())
            {
                collision.GetComponent<BaseEntity>().TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        
    }
}