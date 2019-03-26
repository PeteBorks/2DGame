using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeepController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField, Range(0, 10)]
    float speed = 5f;
    [Header("References")]
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
    LookAt autoLook;


    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        autoLook = GetComponent<LookAt>();
    }

    void Update()
    {
        if (!inputEnabled)
            autoLook.isOn = true;
        else
            autoLook.isOn = false;

        if (inputEnabled && ((movement.x < 0 && !sprite.flipX) || (movement.x > 0 && sprite.flipX)))
            sprite.flipX = !sprite.flipX;

        if (sprite.flipX)
            autoLook.lightsLeft();
        else
            autoLook.lightsRight();

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
