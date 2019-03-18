/**
 * PlayerController.cs
 * Created by: Pedro Borges
 * Created on: 04/03/19 (dd/mm/yy)
 */

using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

[SelectionBase]
public class PlayerController : MonoBehaviour
{
    
    [SerializeField, Range(0,10)]
    float speed = 1f;
    [SerializeField, Range(0,2)]
    float wallJumpDelay = 1f;
    [Range(1, 3)]
    public float crouchModifier = 1.5f;
    [SerializeField]
    Animator animator;
    [SerializeField]
    LayerMask groundFilter;
    [SerializeField]
    LayerMask jumpWall;
    [SerializeField]
    GameObject rightCam;
    [SerializeField]
    GameObject leftCam;
    [SerializeField]
    SpriteRenderer sprite;
    [SerializeField]
    GameObject followPivot;
    [SerializeField, Range (0,1)]
    float jumpThreshold = 0.1f;
    [SerializeField, Range(5, 25)]
    float jumpForce = 5f;
    float crouchDetectPos = 2f;

    bool isFacingRight = true;
    bool isGrounded = true;
    bool isCrouching = false;
    bool crouchCheck = false;
    bool rightSideCheck = false;
    bool leftSideCheck = false;
    bool justJumpR = false;
    bool justJumpL = false;
    bool jump;
    
    
    RaycastHit2D[] results = new RaycastHit2D[1];
    ContactFilter2D filter;
    ContactFilter2D jumpFilter;
    Rigidbody2D rb2d;
    CapsuleCollider2D collider2d;

    Vector2 movement;

    [Header("Events")]
    [Space]
    public UnityEvent CrouchEvent;

    void Start()
    {
        filter = new ContactFilter2D()
        {
            useLayerMask = false,
            layerMask = groundFilter,
            maxDepth = 1,
            minDepth = -1
        };

        jumpFilter = new ContactFilter2D()
        {
            useLayerMask = true,
            layerMask = jumpWall,
            maxDepth = 1,
            minDepth = -1
        };
        rb2d = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        

        movement = new Vector2(Input.GetAxis("Horizontal") * speed, rb2d.velocity.y);
        
        animator.SetFloat("speed", Mathf.Abs(movement.x));

        if ((movement.x < 0 && isFacingRight) || (movement.x > 0 && !isFacingRight))
            SwitchDirection();

        if (Input.GetButtonDown("Crouch") && isGrounded && !crouchCheck)
            Crouch();

        if (Input.GetButtonDown("Jump") && (isGrounded || animator.GetBool("isGrabbing")) && !isCrouching)
        {
            jump = true;
            animator.SetBool("isOnAir", true);
        }

        
    }

    void FixedUpdate()
    {
        bool wasGrounded = isGrounded;
        
        if (jump && animator.GetBool("isGrabbing"))
        {
            if (rightSideCheck)
            {
                rb2d.simulated = true;
                movement = new Vector2(-15, 14);
                justJumpR = true;                //TODO delete justJUMP
                justJumpL = false;
                StartCoroutine(JustJump(true));
            }
            if(leftSideCheck)
            {
                rb2d.simulated = true;
                movement = new Vector2(15, 14);
                justJumpL = true;                //TODO delete justJUMP
                justJumpR = false;
                StartCoroutine(JustJump(false));
            }
            animator.SetBool("isGrabbing", false);
            jump = false;
        }
        else if(jump)
        {
            movement.y = jumpForce;
            jump = false;
        }

        isGrounded = collider2d.Raycast(Vector2.down, filter, results, collider2d.bounds.extents.y + jumpThreshold) == 1;
        crouchCheck = collider2d.Raycast(Vector2.up, filter, results, collider2d.bounds.extents.y + collider2d.bounds.extents.y) == 1;
        rightSideCheck = collider2d.Raycast(Vector2.right, jumpFilter, results, collider2d.bounds.extents.x + 0.05f) == 1;
        leftSideCheck = collider2d.Raycast(Vector2.left, jumpFilter, results, collider2d.bounds.extents.x + 0.05f) == 1;

        if (wasGrounded && !isGrounded)
            animator.SetBool("isOnAir", true);
        else if (isGrounded && !wasGrounded)
            OnLanding();

        if (animator.GetBool("isOnAir") && !animator.GetBool("isGrabbing"))
        {
            if (rightSideCheck && !justJumpR)
            {
                animator.SetBool("isGrabbing", true);
                rb2d.simulated = false;
                sprite.flipX = true;
            }     
            if (leftSideCheck && !justJumpL)
            {
                animator.SetBool("isGrabbing", true);
                rb2d.simulated = false;
                sprite.flipX = false;
            }
            
        }
        rb2d.velocity = movement;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * jumpThreshold);
        //DrawLine(transform.position + Vector3.up + Vector3.left, transform.position + Vector3.up + Vector3.left * 0.5f);
    }

    public void OnLanding()
    {
        animator.SetBool("isOnAir", false);
    }
    void SwitchDirection()
    {
        if (isFacingRight)
        {
            leftCam.SetActive(true);
            rightCam.SetActive(false);
            if(!animator.GetBool("isGrabbing"))
                sprite.flipX = true;
        }
        else
        {
            leftCam.SetActive(false);
            rightCam.SetActive(true);
            if (!animator.GetBool("isGrabbing"))
                sprite.flipX = false;
        }
        
        isFacingRight = !isFacingRight;
    }

    //TODO check hardcoded values
    void Crouch()
    {
        if(!isCrouching)
        {
            collider2d.size = new Vector2(1.3f,1.3f);   
            collider2d.offset = new Vector2(0.0f, 0.65f);
            speed /= crouchModifier;
            followPivot.transform.position -= new Vector3(0, 1, 0);
        }
        else
        {
            collider2d.size = new Vector2(1.5f, 2.0f);
            collider2d.offset = new Vector2(0.0f, 1.0f);
            speed *= crouchModifier;
            followPivot.transform.position += new Vector3(0, 1, 0);
        }
        isCrouching = !isCrouching;
        CrouchEvent.Invoke();
    }

    IEnumerator JustJump(bool right)
    {
        yield return new WaitForSeconds(wallJumpDelay);
        if (right)
            justJumpR = false;
        if (!right)
            justJumpL = false;
    }
}
