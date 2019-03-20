using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeepController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField, Range(0, 10)]
    float speed = 5f;
    [SerializeField]
    public GameObject mainCam;
    [SerializeField]
    Main mainScript;
    [SerializeField]
    SpriteRenderer sprite;
    public Collider2D collider2D5;
    [HideInInspector]
    public bool inputEnabled = false;
    float dirNum;
    public Rigidbody2D rb2D;
    Vector2 movement;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // toggles sides when following depending on player position
        if(!inputEnabled)
        {
            Vector3 heading = mainScript.playerPawn.transform.position - transform.position;
            switch (AngleDir(transform.forward, heading, transform.up))
            {
                case 1:
                    sprite.flipX = false;
                    break;
                case -1:
                    sprite.flipX = true;
                    break;
            }
        }

        if (inputEnabled && ((movement.x < 0 && !sprite.flipX) || (movement.x > 0 && sprite.flipX)))
            sprite.flipX = !sprite.flipX;

            if (inputEnabled && Input.GetButtonDown("ChangePawn"))
        {
            mainScript.ChangePawn(1);
        }
    }

    private void FixedUpdate()
    {
        if (inputEnabled)
        {
            movement = new Vector2(Input.GetAxis("Horizontal") * speed, Input.GetAxis("Vertical") * speed);
            rb2D.AddForce(Vector2.ClampMagnitude(movement * 40, 40));
        }
        
    }
    
    float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);
        if (dir > 0f)
        {
            return 1f;
        }
        else if (dir < 0f)
        {
            return -1f;
        }
        else
        {
            return 0f;
        }
    }

    public void DisableFollowing()
    {
        GetComponent<Pathfinding.AIDestinationSetter>().enabled = false;
        GetComponent<Pathfinding.AIPath>().enabled = false;
        GetComponent<Pathfinding.SimpleSmoothModifier>().enabled = false;
        GetComponent<Pathfinding.FunnelModifier>().enabled = false;
    }

    public void EnableFollowing()
    {
        GetComponent<Pathfinding.AIDestinationSetter>().enabled = true;
        GetComponent<Pathfinding.AIPath>().enabled = true;
        GetComponent<Pathfinding.SimpleSmoothModifier>().enabled = true;
        GetComponent<Pathfinding.FunnelModifier>().enabled = true;
    }
}
