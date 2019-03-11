/**
 * PlayerController.cs
 * Created by: Pedro Borges
 * Created on: 04/03/19 (dd/mm/yy)
 */

using Cinemachine;
using UnityEngine;

[SelectionBase]
public class PlayerController : MonoBehaviour
{

    [SerializeField, Range(0,10)]
    float speed = 1;
    [SerializeField]
    LayerMask groundFilter;
    [SerializeField]
    GameObject rightCam;
    [SerializeField]
    GameObject leftCam;
    [SerializeField]
    SpriteRenderer sprite;
    [SerializeField, Range (0,1)]
    float jumpThreshold = 0.1f;
    [SerializeField, Range(5, 10)]
    float jumpForce = 5f;

    bool isFacingRight = true;
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

    void OnRenderObject()
    {
        if ((movement.x < 0 && isFacingRight) || (movement.x > 0 && !isFacingRight))
            SwitchDirection();
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

    void SwitchDirection()
    {
        if (isFacingRight)
        {
            leftCam.SetActive(true);
            rightCam.SetActive(false);
            
        }
        else
        {
            leftCam.SetActive(false);
            rightCam.SetActive(true);
        }
        sprite.transform.localScale = new Vector3(-sprite.transform.localScale.x, sprite.transform.localScale.y, sprite.transform.localScale.z);
        sprite.flipX = !sprite.flipX;
        isFacingRight = !isFacingRight;
    }
}
