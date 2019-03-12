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
    [SerializeField, Range(1, 3)]
    float crouchModifier = 1.5f;
    [SerializeField]
    Animator animator;
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
    [SerializeField, Range(5, 25)]
    float jumpForce = 5f;

    bool isFacingRight = true;
    bool isGrounded;
    bool isCrouching = false;
    bool jump;
    
    RaycastHit2D[] results = new RaycastHit2D[1];
    ContactFilter2D filter;
    Rigidbody2D rb2d;
    CapsuleCollider2D collider2d;
    
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
        collider2d = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        isGrounded = collider2d.Raycast(Vector2.down, filter, results, collider2d.bounds.extents.y + jumpThreshold) == 1;
        movement = new Vector2(Input.GetAxis("Horizontal") * speed, rb2d.velocity.y);
        
        animator.SetFloat("speed", Mathf.Abs(movement.x));

        if ((movement.x < 0 && isFacingRight) || (movement.x > 0 && !isFacingRight))
            SwitchDirection();

        if (Input.GetButtonDown("Crouch") && isGrounded)
            Crouch();

        if (Input.GetButtonDown("Jump") && isGrounded && !isCrouching)
            jump = true;
    }

    void FixedUpdate()
    {
        if(jump)
        {
            movement.y = jumpForce;
            jump = false;
            Debug.Log("Pulou");
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
            sprite.flipX = true;
        }
        else
        {
            leftCam.SetActive(false);
            rightCam.SetActive(true);
            sprite.flipX = false;
        }
        
        isFacingRight = !isFacingRight;
    }

    void Crouch()
    {
        if(!isCrouching)
        {
            collider2d.size = new Vector2(1.3f,1.3f);
            collider2d.offset = new Vector2(0.0f, 0.65f);
            speed /= crouchModifier;
        }
        else
        {
            collider2d.size = new Vector2(1.5f, 2.0f);
            collider2d.offset = new Vector2(0.0f, 1.0f);
            speed *= crouchModifier;
        }
        isCrouching = !isCrouching;
    }
}
