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
    [SerializeField, Range(0, 10)]
    float speed = 1f;
    [SerializeField, Range(0, 2)]
    float wallJumpDelay = 1f;
    [SerializeField, Range(0, 30)]
    float dashSpeed = 14;
    [SerializeField, Range(0, 1)]
    float dashDuration = 0.2f;
    [SerializeField, Range(0, 4)]
    float dashCooldown = 0.5f;
    [SerializeField, Range(1, 2)]
    float slideSpeedModifier = 1.3f;
    [SerializeField, Range(0, 1)]
    float slideDuration = 0.5f;
    [SerializeField, Range(0, 4)]
    float slideCooldown = 1f;
    [SerializeField, Range(0, 1)]
    float jumpThreshold = 0.1f;
    [SerializeField, Range(5, 25)]
    float jumpForce = 5f;
    [SerializeField, Range(0, 2)]
    float fireCooldown = 0.3f;

    [Header("References")]
    [SerializeField]
    Main mainScript;
    [SerializeField]
    public Animator animator;
    [SerializeField]
    LayerMask groundFilter;
    [SerializeField]
    LayerMask jumpWall;
    [SerializeField]
    public GameObject rightCam;
    [SerializeField]
    public GameObject leftCam;
    [SerializeField]
    SpriteRenderer sprite;
    [SerializeField]
    GameObject followPivot;
    [SerializeField]
    GameObject bullet;
    [SerializeField]
    GameObject barrelFX;
    [SerializeField]
    GameObject barrelFXSocket;

    [Header("Events")]
    [Space]
    public UnityEvent SlideEvent;

    
    float normalSpeed;
    float gravityScale;
    [HideInInspector]
    public bool inputEnabled = true;
    bool canSlide = true;
    bool canDash = true;
    [HideInInspector]
    public bool isFacingRight = true;
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
    bool isOnTrigger = false;
    bool isRotSliding;
    GameObject interactableObject;
    RaycastHit2D[] results = new RaycastHit2D[1];
    ContactFilter2D filter;
    ContactFilter2D jumpFilter;
    [HideInInspector]
    public Rigidbody2D rb2D;
    CapsuleCollider2D collider2d;
    Vector3 barrelFXDefaultPos;
    Vector2 movement;
    RaycastHit2D hit;
    Vector3 axis;
    float angle;


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

        barrelFXDefaultPos = barrelFXSocket.transform.localPosition;
        rb2D = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<CapsuleCollider2D>();
        normalSpeed = speed;
        gravityScale = rb2D.gravityScale;
    }

    void Update()
    {
        bool wasGrounded = isGrounded;

        // for com array de pontos de origem
        isGrounded = ((Physics2D.Raycast(transform.position + -transform.up * 0.1f, -transform.up, filter, results, jumpThreshold)) == 1 ||
                      (Physics2D.Raycast(transform.position + transform.right * 0.4f + -transform.up * 0.1f, -transform.up, filter, results,  jumpThreshold) == 1) ||
                      (Physics2D.Raycast(transform.position + -transform.right * 0.4f + - transform.up * 0.1f, -transform.up, filter, results,  jumpThreshold) == 1));
        Debug.Log(Physics2D.Raycast(transform.position + -transform.up * 0.1f, -transform.up, filter, results, jumpThreshold));
        ceilingCheck = collider2d.Raycast(Vector2.up, filter, results, collider2d.bounds.extents.y + collider2d.bounds.extents.y) == 1;
        rightSideCheck = collider2d.Raycast(Vector2.right, jumpFilter, results, collider2d.bounds.extents.x + 0.1f) == 1;
        leftSideCheck = collider2d.Raycast(Vector2.left, jumpFilter, results, collider2d.bounds.extents.x + 0.1f) == 1;
        

        if (inputEnabled)
            movement = new Vector2(Input.GetAxis("Horizontal") * speed, rb2D.velocity.y);
        
        animator.SetFloat("speed", Mathf.Abs(movement.x));

        if ((movement.x < 0 && isFacingRight) || (movement.x > 0 && !isFacingRight))
            SwitchDirection();

        if (inputEnabled && Input.GetButtonDown("Fire3") && !ceilingCheck)
            if (animator.GetBool("isOnAir") && canDash)
                StartCoroutine("Dash");
            else if (isGrounded && canSlide)
                StartCoroutine("Slide");

        if ((inputEnabled || (animator.GetBool("isSliding") && !ceilingCheck || isRotSliding)) && Input.GetButtonDown("Jump") && (isGrounded || animator.GetBool("isGrabbing")))
        {
            jump = true;
            animator.SetBool("isOnAir", true);
            if (isSliding)
            {
                StartCoroutine(StopSliding());
            }
        }

        if(inputEnabled && Input.GetButtonDown("Fire1") && isGrounded)
        {
            animator.SetTrigger("fire");
            Instantiate(barrelFX, barrelFXSocket.transform);
            GameObject shot = Instantiate(bullet, barrelFXSocket.transform.position, Quaternion.identity);
            if(isFacingRight)
                shot.GetComponent<Bullet>().isRight = true;

            StartCoroutine(FireDelay());

        }

        if (wantToStandUp && !ceilingCheck)
        {
            StartCoroutine(StopSliding());
            wantToStandUp = false;
        }

        if (wasGrounded && !isGrounded && !isRotSliding)
            animator.SetBool("isOnAir", true);
        else if (isGrounded && !wasGrounded && !isRotSliding)
            OnLanding();

        if (!isGrounded && isSliding)
        {
            StartCoroutine(StopSliding());
        }

        if(isDashing && animator.GetBool("isGrabbing"))
        {
            StartCoroutine(StopDashing());
        }

        if (inputEnabled && Input.GetButtonDown("ChangePawn") && ((movement.x < 0.1f && isGrounded) || animator.GetBool("isGrabbing")))
        {
            mainScript.ChangePawn(2);
        }

        if (inputEnabled &&Input.GetButtonDown("Interact") && isOnTrigger)
        {
            StartCoroutine(interactableObject.GetComponent<Button>().OnInteract());
        }

        hit = Physics2D.Raycast(transform.position + Vector3.up * 0.7f + Vector3.left * 0.51f, Vector3.down,0.5f);
        axis = Vector3.Cross(-transform.up, -hit.normal);
        if (axis != Vector3.zero)
        {
            angle = Mathf.Atan2(Vector3.Magnitude(axis), Vector3.Dot(-transform.up, -hit.normal));
            transform.RotateAround(axis,angle);
            inputEnabled = false;
            animator.SetBool("isSliding", true);
            isRotSliding = true;
            rb2D.velocity = Vector2.zero;
            movement = Vector2.zero;
            
        }
        else if(!isGrounded && !hit)
        {
            animator.SetBool("isSliding", false);
            isRotSliding = false;
            inputEnabled = true;
            transform.rotation = Quaternion.identity;
        }
        else if (isRotSliding)
        {
            movement = new Vector2(9, -6.7f);
        }      
    }

    void FixedUpdate()
    {
        
        if (jump && animator.GetBool("isGrabbing"))
        {
            if (rightSideCheck)
            {
                rb2D.simulated = true;
                movement = new Vector2(-speed, jumpForce);
                justJumpR = true;
                justJumpL = false;
                StartCoroutine(JustJump(true));
                if (Input.GetAxis("Horizontal") > 0.1f)
                    sprite.flipX = false;
            }
            if (leftSideCheck)
            {
                rb2D.simulated = true;
                movement = new Vector2(speed, jumpForce);
                justJumpL = true;
                justJumpR = false;
                StartCoroutine(JustJump(false));
                if (Input.GetAxis("Horizontal") < -0.1f)
                    sprite.flipX = true;
            }
            animator.SetBool("isGrabbing", false);
            jump = false;
        }
        else if (jump)
        {
            movement.y = jumpForce; // AddForce
            jump = false;
        }

        if ((animator.GetBool("isOnAir") || isDashing) && !animator.GetBool("isGrabbing"))
        {
            if (rightSideCheck && !justJumpR)
            {
                animator.SetBool("isGrabbing", true);
                rb2D.simulated = false;
                sprite.flipX = true;
            }     
            if (leftSideCheck && !justJumpL)
            {
                animator.SetBool("isGrabbing", true);
                rb2D.simulated = false;
                sprite.flipX = false;
            }
            
        }
        if(!isDashing)
            rb2D.velocity = movement;

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        isOnTrigger = true;
        interactableObject = collision.gameObject;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        isOnTrigger = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + -transform.up * jumpThreshold);
        Gizmos.DrawLine(transform.position + new Vector3(0.4f, 0, 0), transform.position + -transform.up  * jumpThreshold + new Vector3(0.4f, 0, 0));
        Gizmos.DrawLine(transform.position + new Vector3(-0.4f, 0, 0), transform.position + -transform.up  * jumpThreshold + new Vector3(-0.4f, 0, 0));
        //Gizmos.DrawSphere(hit.point, 0.1f);
    }

    public void OnLanding()
    {
        animator.SetBool("isOnAir", false);
    }

    void SwitchDirection()
    {
        if (isFacingRight)
        {
            barrelFXSocket.transform.localScale = new Vector3(-1, 1, 1);
            barrelFXSocket.transform.localPosition = new Vector3(-barrelFXDefaultPos.x, barrelFXDefaultPos.y, barrelFXDefaultPos.z);
            leftCam.SetActive(true);
            rightCam.SetActive(false);
            if(!animator.GetBool("isGrabbing"))
                sprite.flipX = true;
        }
        else
        {
            barrelFXSocket.transform.localScale = new Vector3(1, 1, 1);
            barrelFXSocket.transform.localPosition = barrelFXDefaultPos;
            leftCam.SetActive(false);
            rightCam.SetActive(true);
            if (!animator.GetBool("isGrabbing"))
                sprite.flipX = false;
        }
        
        isFacingRight = !isFacingRight;
    }


    IEnumerator FireDelay()
    {
        inputEnabled = false;
        movement = new Vector2(0, rb2D.velocity.y);
        yield return new WaitForSeconds(fireCooldown);
        inputEnabled = true;
    }
    IEnumerator Dash()
    {
        inputEnabled = false;
        canDash = false;
        movement = Vector2.zero;
        isDashing = true;
        animator.SetBool("dash", true);
        rb2D.velocity = new Vector2(0, 0);
        rb2D.gravityScale = 0;
        if (isFacingRight)
            rb2D.AddForce(new Vector2(dashSpeed, 0), ForceMode2D.Impulse);
        else
            rb2D.AddForce(new Vector2(-dashSpeed, 0), ForceMode2D.Impulse);
        yield return new WaitForSeconds(dashDuration);
        StartCoroutine("StopDashing");
    }
    IEnumerator StopDashing()
    {
        StopCoroutine("Dash");
        movement = Vector2.zero;
        rb2D.gravityScale = gravityScale;
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
            movement = new Vector2(speed, rb2D.velocity.y);
        else
            movement = new Vector2(-speed, rb2D.velocity.y);
        
            
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
        collider2d.size = new Vector2(1, 2);
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
