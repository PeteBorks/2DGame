using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZorgBullet : MonoBehaviour
{
    [SerializeField]
    float speed = 5;
    [SerializeField]
    float damage = 10;
    Rigidbody2D rb;
    Transform target;
    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<PlayerController>().transform;
        rb = GetComponent<Rigidbody2D>();
        transform.right = -(new Vector3(target.position.x, target.position.y + 1.6f, 0) - transform.position);
        rb.velocity = -transform.right * speed;
    }
    void Update()
    {
        target = FindObjectOfType<PlayerController>().transform;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player)
        {
            player.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (!collision.isTrigger)
            Destroy(gameObject);
    }
}
