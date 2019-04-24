using System.Collections;
using UnityEngine;

public class MeepController : MonoBehaviour
{
    public enum State
    {
        Auto,
        Controlled,
        Platform
    }

    [Header("Movement")]
    [SerializeField, Range(0, 10)]
    float speed = 5f;
    [Header("References")]
    public GameObject mainCam;
    public Main mainScript;
    [SerializeField]
    SpriteRenderer sprite;

    [HideInInspector]
    public Collider2D circleCollider;
    [HideInInspector]
    public Collider2D bCollider;
    [HideInInspector]
    public bool inputEnabled = false;
    [HideInInspector]
    public Rigidbody2D rb2D;
    [HideInInspector]
    public State state;

    float dirNum;
    Animator animator;
    Vector2 movement;
    LookAt autoLook;
    Light[] lights;
    Color defaultColor;


    void Start()
    {
        
        lights = GetComponentsInChildren<Light>();
        animator = GetComponentInChildren<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        bCollider = GetComponent<BoxCollider2D>();
        autoLook = GetComponent<LookAt>();
        defaultColor = lights[0].color;
        StartCoroutine(Blink());
    }

    void Update()
    {
        if (state != State.Platform)
            if(sprite.flipX)
                autoLook.lightsLeft();
            else
                autoLook.lightsRight();

        switch (state)
        {
            case State.Auto:
                inputEnabled = false;
                autoLook.isOn = true;
                break;

            case State.Controlled:
                inputEnabled = true;
                autoLook.isOn = false;
                movement = new Vector2(Input.GetAxis("Horizontal") * speed, Input.GetAxis("Vertical") * speed);
                if (((movement.x < 0 && !sprite.flipX) || (movement.x > 0 && sprite.flipX)))
                    sprite.flipX = !sprite.flipX;
                if (inputEnabled && Input.GetButtonDown("ChangePawn"))
                {
                    mainScript.ChangePawn(1);
                    state = State.Auto;
                    EnableFollowing();
                }
                if (inputEnabled && Input.GetButtonDown("Fire1"))
                {
                    state = State.Platform;
                    animator.SetLayerWeight(1, 0);
                    animator.SetBool("isPlatform",true);
                    bCollider.enabled = true;
                    rb2D.bodyType = RigidbodyType2D.Static;
                    for (int i=0; i<lights.Length;i++)
                    {
                        lights[i].color = Color.yellow;
                        if(sprite.flipX)
                        {
                            lights[0].gameObject.transform.localPosition = new Vector3(-0.163f, 0.08f, -0.03f);
                            lights[1].gameObject.transform.localPosition = new Vector3(0.082f, 0.08f, -0.03f);
                        }
                        else
                        {
                            lights[0].gameObject.transform.localPosition = new Vector3(-0.08f, 0.08f, -0.03f);
                            lights[1].gameObject.transform.localPosition = new Vector3(0.16f, 0.08f, -0.03f);
                        }
                        
                    }

                }
                break;

            case State.Platform:
                if (inputEnabled && (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical")))
                {
                    animator.SetBool("isPlatform", false);
                    StartCoroutine(WaitForAnim());
                }
                if (inputEnabled && Input.GetButtonDown("ChangePawn"))
                {
                    mainScript.ChangePawn(1);
                    inputEnabled = false;
                }
                 
                break;
        }

        
    }

    private void FixedUpdate()
    {
        if (state == State.Controlled) 
            rb2D.AddForce(Vector2.ClampMagnitude(movement * 40, 40));
        
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

    IEnumerator Blink()
    {
        yield return new WaitForSeconds(Random.Range(3, 5));
        animator.SetTrigger("blink");           
        StartCoroutine(Blink());
        
    }

    IEnumerator WaitForAnim()
    {
        float t = 0;
        while (t < animator.GetCurrentAnimatorStateInfo(0).length)
        {
            t += Time.deltaTime;
            yield return null;
        }
        yield return new WaitUntil(() => t >= animator.GetCurrentAnimatorStateInfo(0).length);
        state = State.Controlled;
        rb2D.bodyType = RigidbodyType2D.Dynamic;
        bCollider.enabled = false;
        for (int i = 0; i < lights.Length; i++)
            lights[i].color = defaultColor;
    }
}
