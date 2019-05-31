using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Zorg : MonoBehaviour
{
    [SerializeField]
    float health = 1000;
    [SerializeField]
    float walkSpeed = 5;
    [SerializeField]
    float jumpForce = 15;
    [SerializeField]
    GameObject bullet;
    [SerializeField]
    Transform bulletPoint;
    [SerializeField]
    Transform jumpPosR;
    [SerializeField]
    Transform jumpPosL;
    [SerializeField]
    Slider healthSlider;

    [HideInInspector] public Vector3 playerPos;
    [HideInInspector] public Vector3 vectorDistance;
    [HideInInspector] public bool canTurn;
    [HideInInspector] public bool facingRight;
    [HideInInspector] public bool jump;
    //[HideInInspector] 
    public bool dash;
    [HideInInspector] public Rigidbody2D rb;

    Vector3 jumpingVector;
    Animator animator;
    int random;
   

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        healthSlider.value = health;
        playerPos = FindObjectOfType<PlayerController>().transform.position;
        vectorDistance = transform.position - playerPos;
    }

    void FixedUpdate()
    {
        if(jump)
        {
            if (random == 0)
                rb.velocity = new Vector2(-walkSpeed * 2f, rb.velocity.y);
            else
                rb.velocity = new Vector2(walkSpeed * 2f, rb.velocity.y);
        }
        else if(dash)
        {
            if (facingRight)
            {
                rb.velocity = new Vector2(walkSpeed * 5f, 0);
            }
            else
                rb.velocity = new Vector2(-walkSpeed * 5f, 0);
        }
    }

    public void Walk()
    {
        rb.gravityScale = 1.8f;
        if (Vector3.Cross(transform.up, vectorDistance).z > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            rb.velocity = new Vector2(walkSpeed, rb.velocity.y);
            facingRight = true;
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
            rb.velocity = new Vector2(-walkSpeed, rb.velocity.y);
            facingRight = false;
        }
            
    }

    public void Dash()
    {
        rb.gravityScale = 0;
        dash = true;
    }
    public void StopWalk()
    {
        rb.velocity = new Vector2(0,rb.velocity.y);
    }

    public void Jump()
    {
        random = Random.Range(0, 2);
        rb.AddForce(Vector2.up * jumpForce * 1000, ForceMode2D.Impulse);
        jump = true;
    }

    public void JumpShoot()
    {
        StartCoroutine(LerpToJump());
    }

    public void Shoot()
    {
        Instantiate(bullet, bulletPoint.position, bulletPoint.rotation);
    }

    public void DisableSimulation()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void EnableSimulation()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.velocity = Vector2.zero;
    }

    public void TakeDamage(float damage)
    {
        if(health > damage)
        {
            health -= damage;
            StartCoroutine(DamageFeedback());
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
    public void RandomBehaviour()
    {
        //animator.SetTrigger("jumpshoot");
        switch (Random.Range(0, 6))
        {
            case 0:
                animator.SetTrigger("dash");
                break;
            case 1:
                animator.SetTrigger("walk");
                break;
            case 3:
                animator.SetTrigger("jump");
                break;
            case 4:
                animator.SetTrigger("laser");
                break;
            case 5:
                animator.SetTrigger("jumpshoot");
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if(player)
        {
            player.TakeDamage(20);
        }
        if (collision.gameObject.CompareTag("Tilemap"))
        {
            animator.SetTrigger("endDash");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //if(collision.gameObject.CompareTag("Tilemap"))
        //{
        //    animator.SetTrigger("endDash");
        //}
    }

    IEnumerator DamageFeedback()
    {
        GetComponentInChildren<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.05f);
        GetComponentInChildren<SpriteRenderer>().color = Color.white;
    }

    IEnumerator LerpToJump()
    {
        float t = 0;
        Vector3 initialPos = transform.position;
        if(facingRight)
            while (t < 1)
            {
                t += Time.deltaTime;
                transform.position = Vector3.Lerp(initialPos, jumpPosL.position, t);
                yield return null;
            }
        else
            while (t < 1)
            {
                t += Time.deltaTime;
                transform.position = Vector3.Lerp(initialPos, jumpPosR.position, t);
                yield return null;
            }
    }
}
