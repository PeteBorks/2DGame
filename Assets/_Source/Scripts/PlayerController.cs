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
    [Header("Movement")]
    [SerializeField, Range(0,10)]
    float speed = 1f;
    [SerializeField, Range(0,2)]
    float wallJumpDelay = 1f;
    [SerializeField, Range(0,30)]
    float dashSpeed = 14;
    [SerializeField, Range(0, 1)]
    float dashDuration = 0.2f;
    [SerializeField, Range(0, 4)]
    float dashCooldown = 0.5f;
    [SerializeField, Range(1,2)]
    float slideSpeedModifier = 1.3f;
    [SerializeField, Range(0,1)]
    float slideDuration = 0.5f;
    [SerializeField, Range(0,4)]
    float slideCooldown = 1f;
    [SerializeField, Range(0, 1)]
    float jumpThreshold = 0.1f;
    [SerializeField, Range(5, 25)]
    float jumpForce = 5f;

    [Header("References")]
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

    [Header("Events")]
    [Space]
    public UnityEvent SlideEvent;

    float normalSpeed;
    float gravityScale;
    public bool inputEnabled = true;
    bool canSlide = true;
    bool canDash = true;
    bool isFacingRight = true;
    bool isGrounded = true;
    bool isSliding = false;
    bool isDashing = false;
    bool ceilingCheck = false;
    bool rightSideCheck = false;
    bool leftSideCheck = false;
    bool wantToStandUp = false;
    bool justJumpR = false;
    bool justJumpL = false;
    bool jump;
    
    
    RaycastHit2D[] results = new RaycastHit2D[1];
    ContactFilter2D filter;
    ContactFilter2D jumpFilter;
    Rigidbody2D rb2d;
    CapsuleCollider2D collider2d;

    Vector2 movement;

    

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
        normalSpeed = speed;
        gravityScale = rb2d.gravityScale;
    }

    void Update()
    {
        bool wasGrounded = isGrounded;

        isGrounded = collider2d.Raycast(Vector2.down, filter, results, collider2d.bounds.extents.y + jumpThreshold) == 1;
        ceilingCheck = collider2d.Raycast(Vector2.up, filter, results, collider2d.bounds.extents.y + collider2d.bounds.extents.y) == 1;
        rightSideCheck = collider2d.Raycast(Vector2.right, jumpFilter, results, collider2d.bounds.extents.x + 0.1f) == 1;
        leftSideCheck = collider2d.Raycast(Vector2.left, jumpFilter, results, collider2d.bounds.extents.x + 0.1f) == 1;

        if (inputEnabled)
            movement = new Vector2(Input.GetAxis("Horizontal") * speed, rb2d.velocity.y);
        
        animator.SetFloat("speed", Mathf.Abs(movement.x));

        if ((movement.x < 0 && isFacingRight) || (movement.x > 0 && !isFacingRight))
            SwitchDirection();

        if (inputEnabled && Input.GetButtonDown("Fire3") && !ceilingCheck)
            if (animator.GetBool("isOnAir") && canDash)
                StartCoroutine("Dash");
            else if (isGrounded && canSlide)
                StartCoroutine("Slide");

        if ((inputEnabled || animator.GetBool("isSliding")) && Input.GetButtonDown("Jump") && (isGrounded || animator.GetBool("isGrabbing")) && !ceilingCheck)
        {
            jump = true;
            animator.SetBool("isOnAir", true);
            if (isSliding)
            {
                StartCoroutine("StopSliding");
            }
        }

        if (wasGrounded && !isGrounded)
            animator.SetBool("isOnAir", true);
        else if (isGrounded && !wasGrounded)
            OnLanding();

        if (!isGrounded && isSliding)
        {
            StartCoroutine("StopSliding");
        }

        if(isDashing && animator.GetBool("isGrabbing"))
        {
            StartCoroutine("StopDashing");
        }
       
    }

    void FixedUpdate()
    {
        if (jump && animator.GetBool("isGrabbing"))
        {
            if (rightSideCheck)
            {
                rb2d.simulated = true;
                movement = new Vector2(-15, jumpForce);
                justJumpR = true;                
                justJumpL = false;
                StartCoroutine(JustJump(true));
            }
            if(leftSideCheck)
            {
                rb2d.simulated = true;
                movement = new Vector2(15, jumpForce);
                justJumpL = true;               
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

        if(wantToStandUp && !ceilingCheck)
        {
            StartCoroutine("StopSliding");
            wantToStandUp = false;
        }
  
        if ((animator.GetBool("isOnAir") || isDashing) && !animator.GetBool("isGrabbing"))
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
        if(!isDashing)
            rb2d.velocity = movement;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * jumpThreshold);
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

    IEnumerator Dash()
    {
        inputEnabled = false;
        canDash = false;
        movement = Vector2.zero;
        isDashing = true;
        animator.SetBool("dash", true);
        rb2d.velocity = new Vector2(0, 0);
        rb2d.gravityScale = 0;
        if (isFacingRight)
            rb2d.AddForce(new Vector2(dashSpeed, 0), ForceMode2D.Impulse);
        else
            rb2d.AddForce(new Vector2(-dashSpeed, 0), ForceMode2D.Impulse);
        yield return new WaitForSeconds(dashDuration);
        StartCoroutine("StopDashing");
    }
    IEnumerator StopDashing()
    {
        StopCoroutine("Dash");
        movement = Vector2.zero;
        rb2d.gravityScale = gravityScale;
        inputEnabled = true;
        isDashing = false;
        animator.SetBool("dash", false);
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    //TODO check hardcoded values
    IEnumerator Slide()
    {
        speed *= slideSpeedModifier;
        canSlide = false;
        isSliding = true;
        inputEnabled = false;
        animator.SetBool("isSliding", true);
        collider2d.direction = CapsuleDirection2D.Horizontal;
        collider2d.offset = new Vector2(0, 0.55f);
        collider2d.size = new Vector2(2.3f, 1.1f);
        
        if (isFacingRight)
            movement = new Vector2(speed, rb2d.velocity.y);
        else
            movement = new Vector2(-speed, rb2d.velocity.y);
        
            
        SlideEvent.Invoke();
        yield return new WaitForSeconds(slideDuration);
        if (ceilingCheck)
            wantToStandUp = true;
        else
            StartCoroutine("StopSliding");
    }
    IEnumerator StopSliding()
    {
        speed = normalSpeed;
        movement = Vector2.zero; 
        StopCoroutine("Slide");
        collider2d.direction = CapsuleDirection2D.Vertical;
        collider2d.offset = new Vector2(0, 1);
        collider2d.size = new Vector2(1.3f, 2);
        animator.SetBool("isSliding", false);
        isSliding = false;
        inputEnabled = true;
        yield return new WaitForSeconds(slideCooldown);
        canSlide = true;
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
