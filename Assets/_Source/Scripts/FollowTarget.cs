using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform target;
    public float speed;
    public float rotateSpeed;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = FindObjectOfType<Zorg>().transform;
    }

    void FixedUpdate()
    {
        Vector2 direction = new Vector2(target.position.x, target.position.y + 1.5f) - rb.position;
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, transform.right).z;
        rb.angularVelocity = -rotateAmount * rotateSpeed;
        rb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Zorg enemy = collision.GetComponent<Zorg>();
        if (enemy)
        {
            enemy.TakeDamage(3);
            Destroy(gameObject);
        }
    }
}
