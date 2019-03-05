using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[SelectionBase]
public class PlayerController : MonoBehaviour
{

    [SerializeField, Range(0,10)]
    float speed = 1;
    [SerializeField]
    LayerMask groundFilter;
    [SerializeField, Range (0,1)]
    float jumpThreshold = 0.1f;
    [SerializeField, Range(5, 10)]
    float jumpForce = 5f;

    bool isGrounded;
    bool jump;
    RaycastHit2D[] results = new RaycastHit2D[1];
    ContactFilter2D filter;
    Rigidbody2D rb2d;
    Collider2D collider2d;
    Vector2 movement;


    void Start()
    {
        filter = new ContactFilter2D()
        {
            useLayerMask = true,
            layerMask = groundFilter,
            maxDepth = 1,
            minDepth = -1
        };
        rb2d = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();
    }

    void Update()
    {

        isGrounded = collider2d.Raycast(Vector2.down, filter, results, collider2d.bounds.extents.y + jumpThreshold) == 1;
        movement = new Vector2(Input.GetAxis("Horizontal") * speed, rb2d.velocity.y);
        if (Input.GetButtonDown("Jump") && isGrounded)
            jump = true;
    }

    void FixedUpdate()
    {
        if(jump)
        {
            movement.y = jumpForce;
            jump = false;
        }
            
        rb2d.velocity = movement;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * jumpThreshold);
    }
}
