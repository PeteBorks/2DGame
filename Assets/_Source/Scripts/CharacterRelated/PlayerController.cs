/**
 * PlayerController.cs
 * Created by: Pedro Borges
 * Created on: 04/03/19 (dd/mm/yy)
 */
 
using System.Collections;
using Cinemachine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(InputManager))]
public class PlayerController : BaseEntity
{
    [Header("Movement")]
    [SerializeField, Range(0, 10)]
    float speed = 1f;
    [Range(0, 2)]
    public float wallJumpDelay = 1f;
    [SerializeField]
    float jumpDelay = 0.1f;
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
    [SerializeField, Range(0, 4)]
    float cameraShakeAmplitude;
    [SerializeField, Range(0, 4)]
    float cameraShakeFrequency;
    [SerializeField]
    PostProcessVolume postProcessVolume;
    

    [Header("References")]
    [SerializeField]
    public Animator animator;
    [SerializeField]
    LayerMask groundFilter;
    [SerializeField]
    LayerMask jumpWall;
    public GameObject rightCam;
    public GameObject leftCam;
    public GameObject middleCam;
    public GameObject DRightCam;
    public GameObject DLeftCam;
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
    [SerializeField]
    GameObject meleeFX;
    [SerializeField]
    ParticleSystem dashParticle;

    ChromaticAberration chromaticAberrationLayer = null;
    Bloom bloomLayer = null;
    InputManager inputManager;
    float normalSpeed;
    float gravityScale;
    //[HideInInspector]
    public bool inputEnabled = true;
    bool canSlide = true;
    bool canDash = true;
    
    public bool isFacingRight = true;
    [HideInInspector]
    public bool isGrounded = true;
    bool isSliding = false;
    bool isDashing = false;
    bool ceilingCheck = false;
    [HideInInspector]
    public bool rightSideCheck = false;
    [HideInInspector]
    public bool leftSideCheck = false;
    bool wantToStandUp = false;
    bool justJumpR = false;
    bool justJumpL = false;
    bool jump;
    bool isOnTrigger = false;
    bool isRotSliding;

    ParticleSystem activatedParticle;
    GameObject interactableObject;
    RaycastHit2D[] results = new RaycastHit2D[1];
    ContactFilter2D filter;
    ContactFilter2D jumpFilter;
    [HideInInspector]
    public Rigidbody2D rb2D;
    CapsuleCollider2D collider2d;
    Vector3 barrelFXDefaultPos;
    Vector3 barrelFXJumpingPos;
    GameObject target;
    Main mainScript;
    
    public Vector2 movement;
    RaycastHit2D groundHit;
    RaycastHit2D meleeCheck;
    Vector3 axis;
    [HideInInspector]
    public CinemachineVirtualCamera currentCam;
    CinemachineBasicMultiChannelPerlin camNoise;
    float angle;
    bool wasGrounded;

    void Start()
    {
        mainScript = FindObjectOfType<Main>();
        inputManager = GetComponent<InputManager>();
        currentCam = rightCam.GetComponent<CinemachineVirtualCamera>();
        camNoise = currentCam.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
        postProcessVolume.profile.TryGetSettings(out chromaticAberrationLayer);
        postProcessVolume.profile.TryGetSettings(out bloomLayer);
        filter = new ContactFilter2D()
        {
            useLayerMask = true,
            layerMask = groundFilter,
            maxDepth = 10,
            minDepth = -10
        };

        jumpFilter = new ContactFilter2D()
        {
            useLayerMask = true,
            layerMask = jumpWall,
            maxDepth = 1,
            minDepth = -1
        };

        barrelFXDefaultPos = barrelFXSocket.transform.localPosition;
        barrelFXJumpingPos = new Vector3(barrelFXSocket.transform.localPosition.x, barrelFXSocket.transform.localPosition.y + 0.3f, barrelFXSocket.transform.localPosition.z);
        rb2D = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<CapsuleCollider2D>();
        normalSpeed = speed;
        gravityScale = rb2D.gravityScale;
    }

    void Update()
    {
        wasGrounded = isGrounded;
        isGrounded = ((Physics2D.Raycast(transform.position + -transform.up * 0.1f, -transform.up, filter, results, jumpThreshold)) == 1 ||
                      (Physics2D.Raycast(transform.position + transform.right * 0.4f + -transform.up * 0.1f, -transform.up, filter, results, jumpThreshold) == 1) ||
                      (Physics2D.Raycast(transform.position + -transform.right * 0.4f + -transform.up * 0.1f, -transform.up, filter, results, jumpThreshold) == 1));
        ceilingCheck = collider2d.Raycast(Vector2.up, filter, results, collider2d.bounds.extents.y + collider2d.bounds.extents.y) == 1;
        rightSideCheck = collider2d.Raycast(Vector2.right, jumpFilter, results, collider2d.bounds.extents.x + 0.1f) == 1;
        leftSideCheck = collider2d.Raycast(Vector2.left, jumpFilter, results, collider2d.bounds.extents.x + 0.1f) == 1;
        meleeCheck = Physics2D.Raycast(transform.position + Vector3.up + Vector3.left * 1.25f, transform.right, 2.5f, LayerMask.GetMask("Enemy"));

        if (meleeCheck)
        {
            target = meleeCheck.collider.gameObject;
            target.transform.GetChild(0).gameObject.SetActive(true);
        }
        else if(target)
            target.transform.GetChild(0).gameObject.SetActive(false);

        if (inputEnabled && inputManager.dash)
            if (animator.GetBool("isOnAir") && !animator.GetBool("isGrabbing") && canDash)
                StartCoroutine("Dash");
            else if (isGrounded && canSlide)
                StartCoroutine("Slide");

        if ((inputEnabled || (animator.GetBool("isSliding") && !ceilingCheck || isRotSliding)) && inputManager.jump && (isGrounded || animator.GetBool("isGrabbing")))
        {
            AudioManager.instance.PlaySound("shouts", transform.position, 0.2f, 0.98f);
            if (isGrounded && !isSliding && !isRotSliding)
            {
                StartCoroutine(Jump());
                animator.SetTrigger("jump");

            }
            else
            {
                AudioManager.instance.PlaySound("jumponwall", transform.position, 0.4f);
                StartCoroutine(Jump());
            }
                
            inputEnabled = true;
            if (isFacingRight)
                barrelFXSocket.transform.localPosition = barrelFXJumpingPos;
            else
                barrelFXSocket.transform.localPosition = new Vector3(-barrelFXJumpingPos.x, barrelFXJumpingPos.y, barrelFXJumpingPos.z);
            
        }

        if(!animator.GetBool("isGrabbing"))
            if (isFacingRight && sprite.flipX)
                sprite.flipX = false;
            else if (!isFacingRight && !sprite.flipX)
                sprite.flipX = true;

        groundHit = Physics2D.Raycast(transform.position + Vector3.up * 0.7f + Vector3.left * 0.51f, Vector3.down, 0.5f, LayerMask.GetMask("Default"));
        axis = Vector3.Cross(-transform.up, -groundHit.normal);

        if (axis != Vector3.zero && !groundHit.collider.CompareTag("Enemy"))
        {
            angle = Mathf.Atan2(Vector3.Magnitude(axis), Vector3.Dot(-transform.up, -groundHit.normal));
            transform.RotateAround(axis, angle);
            inputEnabled = false;
            animator.SetBool("isSliding", true);
            isRotSliding = true;
            rb2D.velocity = Vector2.zero;
            movement = Vector2.zero;

        }
        else if ((!isGrounded && !groundHit) && health!=0 && !animator.GetBool("isGrabbing") && !isDashing)
        {
            animator.SetBool("isSliding", false);
            isRotSliding = false;
            inputEnabled = true;
            animator.SetBool("isOnAir", true);
            transform.rotation = Quaternion.identity;
        }
        else if (isRotSliding)
        {
            movement = new Vector2(5.5f, -6.7f);
        }

        if (inputEnabled && inputManager.fire && !animator.GetBool("isGrabbing"))
        {
            if(meleeCheck && !animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Melee") && isGrounded)
            { 
                target.GetComponent<EnemyPatrol>().MeleeDeath();
                Instantiate(meleeFX, barrelFXSocket.transform);
                movement = Vector2.zero;
                animator.SetTrigger("melee");
                inputEnabled = false;
                StartCoroutine(WaitMelee());
            }
            else if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Melee"))
            {
                Instantiate(barrelFX, barrelFXSocket.transform);
                GameObject shot = Instantiate(bullet, barrelFXSocket.transform.position, Quaternion.identity);
                animator.SetTrigger("fire");
                StartCoroutine(FireDelay());
                if (isFacingRight)
                    shot.GetComponent<Bullet>().isRight = true;
            }
            
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

        if (inputEnabled && inputManager.changePawn && ((movement.x < 0.1f && movement.x > -0.1f && isGrounded) || animator.GetBool("isGrabbing")))
        {
            mainScript.ChangePawn(2);
        }

        if (inputEnabled && inputManager.interact && isOnTrigger)
        {
            StartCoroutine(interactableObject.GetComponent<ButtonAction>().OnInteract());
        }

        
        
    }

    void FixedUpdate()
    {
        if (inputEnabled)
            movement = new Vector2(inputManager.horizontalAxis * speed, rb2D.velocity.y);
        else if(!isSliding && !isRotSliding)
            movement = new Vector2(0,rb2D.velocity.y);
        if ((movement.x < 0 && isFacingRight) || (movement.x > 0 && !isFacingRight))
            SwitchDirection();

        animator.SetFloat("speed", Mathf.Abs(movement.x));
        
        if (mainScript.currentPawn==Main.CurrentPawn.Carrie && jump && animator.GetBool("isGrabbing"))
        {
            Detach(true);
        }
        else if (jump)
        {
            if (isSliding)
            {
                StartCoroutine(StopSliding());
            }
            movement.y = jumpForce; // AddForce
            jump = false;         
        }

        
        if ((animator.GetBool("isOnAir") || isDashing) && !animator.GetBool("isGrabbing"))
        {
            if (rightSideCheck && !justJumpR)
            {
                AudioManager.instance.PlaySound("walljump", transform.position, 0.4f);
                if (isDashing)
                    StartCoroutine(StopDashing());
                animator.SetBool("isFiring", false);
                animator.SetBool("isGrabbing", true);
                rb2D.velocity = Vector2.zero;
                movement = Vector2.zero;
                rb2D.gravityScale = 0;
                sprite.flipX = true;
            }     
            if (leftSideCheck && !justJumpL)
            {
                AudioManager.instance.PlaySound("walljump", transform.position, 0.4f);
                if (isDashing)
                    StartCoroutine(StopDashing());
                animator.SetBool("isFiring", false);
                animator.SetBool("isGrabbing", true);
                rb2D.velocity = Vector2.zero;
                movement = Vector2.zero;
                rb2D.gravityScale = 0;
                sprite.flipX = false;
            }
            
            
        }
        if(!isDashing && rb2D.gravityScale != 0)
            rb2D.velocity = movement;

    }

    public void Detach(bool useJump)
    {
        if (rightSideCheck)
        {
            RightSideDetach();
            if(useJump)
                movement = new Vector2(-speed, jumpForce);
        }
        if (leftSideCheck)
        {
            LeftSideDetach();
            if (movement.x < -0.1f)
                sprite.flipX = true;
            if(useJump)
                movement = new Vector2(speed, jumpForce);
        }
        animator.SetBool("isGrabbing", false);
        jump = false;
    }

    public void LeftSideDetach()
    {
        rb2D.gravityScale = 3.25f;
        justJumpL = true;
        justJumpR = false;
        StartCoroutine(JustJumped(false));
    }

    public void RightSideDetach()
    {
        rb2D.gravityScale = 3.25f;
        justJumpR = true;
        justJumpL = false;
        StartCoroutine(JustJumped(true));
        if (Input.GetAxis("Horizontal") > 0.1f)
            sprite.flipX = false;
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
        Gizmos.DrawLine(transform.position + Vector3.up + Vector3.left * 1.25f, transform.position + Vector3.up +transform.right * 1.25f);
        //Gizmos.DrawSphere(hit.point, 0.1f);
    }

    public void OnLanding()
    {
        animator.SetBool("isOnAir", false);
        animator.SetBool("isFiring", false);
        if(isFacingRight)
            barrelFXSocket.transform.localPosition = barrelFXDefaultPos;
        else
            barrelFXSocket.transform.localPosition = new Vector3(-barrelFXDefaultPos.x, barrelFXDefaultPos.y, barrelFXDefaultPos.z);
    }

    public void Hit()
    {
        AudioManager.instance.PlaySound("hit", transform.position, 0.2f);
        GetComponent<VignetteFeedback>().Hit();
            if(movement.x == 0)
                animator.SetTrigger("hit");
    }

    void SwitchDirection()
    {
        if (isFacingRight)
        {
            barrelFXSocket.transform.localScale = new Vector3(-1, 1, 1);
            if (isGrounded)
                barrelFXSocket.transform.localPosition = new Vector3(-barrelFXDefaultPos.x, barrelFXDefaultPos.y, barrelFXDefaultPos.z);
            else
                barrelFXSocket.transform.localPosition = new Vector3(-barrelFXJumpingPos.x, barrelFXJumpingPos.y, barrelFXJumpingPos.z);
            if(!middleCam.activeSelf && mainScript.currentPawn==Main.CurrentPawn.Carrie)
            {
                rightCam.SetActive(false);
                leftCam.SetActive(true);
                currentCam = leftCam.GetComponent<CinemachineVirtualCamera>();
            }
            
            if(!animator.GetBool("isGrabbing"))
            {
                sprite.flipX = true;

            }
                
        }
        else
        {
            barrelFXSocket.transform.localScale = new Vector3(1, 1, 1);
            barrelFXSocket.transform.localPosition = barrelFXDefaultPos;
            if(!middleCam.activeSelf && mainScript.currentPawn==Main.CurrentPawn.Carrie)
            {
                leftCam.SetActive(false);
                rightCam.SetActive(true);
                currentCam = rightCam.GetComponent<CinemachineVirtualCamera>();
            }
            
            if (!animator.GetBool("isGrabbing"))
            {
                sprite.flipX = false;
            }
                
        }
        camNoise = currentCam.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
        isFacingRight = !isFacingRight;
    }

    IEnumerator WaitMelee()
    {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.87f);
            inputEnabled = true;
    }
    IEnumerator FireDelay()
    {
        animator.SetBool("isFiring", true);
        AudioManager.instance.PlaySound("carriefire", transform.position, 0.5f);
        chromaticAberrationLayer.intensity.value = 0.45f;
        bloomLayer.intensity.value = 7;
        camNoise.m_AmplitudeGain = cameraShakeAmplitude;
        camNoise.m_FrequencyGain = cameraShakeFrequency;
        inputEnabled = false;
        movement = new Vector2(0, rb2D.velocity.y);
        yield return new WaitForSeconds(fireCooldown);
        chromaticAberrationLayer.intensity.value = 0.1f;
        bloomLayer.intensity.value = 6;
        camNoise.m_AmplitudeGain = 0;
        camNoise.m_FrequencyGain = 0;
        inputEnabled = true;
        if(isGrounded)
            animator.SetBool("isFiring", false);
    }

    IEnumerator Jump()
    {
        
        yield return new WaitForSeconds(jumpDelay/2);
        jump = true;
        yield return new WaitForSeconds(jumpDelay / 2);
        animator.SetBool("isOnAir", true);
    }
    IEnumerator Dash()
    {
        AudioManager.instance.PlaySound("dash", transform.position, 0.4f);
        chromaticAberrationLayer.intensity.value = 0.45f;
        isDashing = true;
        inputEnabled = false;
        canDash = false;
        movement = Vector2.zero;
        animator.SetBool("dash", true);
        animator.SetBool("isFiring", false);
        rb2D.velocity = new Vector2(0, 0);
        rb2D.gravityScale = 0;
        if (isFacingRight)
            rb2D.AddForce(new Vector2(dashSpeed, 0), ForceMode2D.Impulse);
        else
            rb2D.AddForce(new Vector2(-dashSpeed, 0), ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.05f);
        ParticleSystemRenderer particle = dashParticle.GetComponent<ParticleSystemRenderer>();
        if (isFacingRight)
            particle.flip = new Vector3(0, 0, 0);
        else
            particle.flip = new Vector3(1, 0, 0);
        activatedParticle = Instantiate(dashParticle, transform.position + Vector3.up * 1, Quaternion.identity, transform);
        yield return new WaitForSeconds(dashDuration);
        StartCoroutine("StopDashing");
    }
    IEnumerator StopDashing()
    {
        chromaticAberrationLayer.intensity.value = 0.1f;
        StopCoroutine("Dash");
        movement = Vector2.zero;
        dashParticle.Stop();
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
        collider2d.offset = new Vector2(0, 0.5f);
        collider2d.size = new Vector2(1f, 1f);
        
        if (isFacingRight)
            movement = new Vector2(speed, rb2D.velocity.y);
        else
            movement = new Vector2(-speed, rb2D.velocity.y);
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
        if(activatedParticle)
            activatedParticle.Stop();
        yield return new WaitForSeconds(slideCooldown);
        canSlide = true;
        if(activatedParticle)
        Destroy(activatedParticle.gameObject);

    }
    IEnumerator JustJumped(bool right)
    {
        yield return new WaitForSeconds(wallJumpDelay);
        if (right)
            justJumpR = false;
        if (!right)
            justJumpL = false;
    }
}
