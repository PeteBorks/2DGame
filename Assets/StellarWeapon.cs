using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StellarWeapon : MonoBehaviour
{
    [SerializeField]
    Transform bulletPoint;
    [SerializeField]
    GameObject bullet;
    Transform target;
    SpriteRenderer sprite;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<Zorg>().transform;
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Instantiate(bullet, bulletPoint.position, bulletPoint.rotation);
        }
    }

    void FixedUpdate()
    {
        if(target)
        {
            Vector2 direction = new Vector2(target.position.x, target.position.y + 1.5f) - rb.position;
            direction.Normalize();
            transform.right = direction;
        }
        


    }
}
