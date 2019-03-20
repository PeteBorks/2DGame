using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeepController : MonoBehaviour
{

    [SerializeField]
    GameObject target;
    [SerializeField]
    PlayerController pawnController;
    [SerializeField]
    float distanceThreshold = 2.0f;
    [SerializeField]
    SpriteRenderer sprite;
    public float speed = 9.0f;
    float slideSpeed, normalSpeed;
    float distance;
    bool isFacingRight = true;
    Vector2 movement;
    Rigidbody2D rb2d;
    
    void Start()
    {
        
        normalSpeed = speed;
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
        if ((movement.x < 0 && isFacingRight) || (movement.x > 0 && !isFacingRight))
            SwitchDirection();
        distance = Vector3.Distance(transform.position, target.transform.position);
        

    }

    void FixedUpdate()
    {
       // if (distance > distanceThreshold)
        //    transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
    }

    void SwitchDirection()
    {
        if (isFacingRight)
        {
            sprite.flipX = true;
        }
        else
        {
            sprite.flipX = false;
        }

        isFacingRight = !isFacingRight;
    }
    public void OnMasterCrouch()
    {
        if (speed == normalSpeed)
        {
            speed = slideSpeed;
        }
        else
        {
            speed = normalSpeed;
        }
        
        
    }
}
