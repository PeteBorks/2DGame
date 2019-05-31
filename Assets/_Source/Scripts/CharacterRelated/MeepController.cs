using System.Collections;
using UnityEngine;

public class MeepController : MonoBehaviour
{
    public enum State
    {
        Auto,
        Controlled,
        Platform,
        Rage
    }

    [Header("Parameters")]
    [Range(0, 10)]
    public float speed = 5f;
    [SerializeField]
    float distanceLimit = 15;
    [SerializeField]
    float distanceToReset = 10;
    public bool canPlatform;
    [Header("References")]
    public GameObject mainCam;
    public GameObject closeCam;
    public Main mainScript;
    [SerializeField]
    AudioClip [] sounds;
    [SerializeField]
    SpriteRenderer sprite;
    [SerializeField]
    Sprite rageSprite;

    [HideInInspector]
    public GameObject defaultCam;
    [HideInInspector]
    public Collider2D circleCollider;
    [HideInInspector]
    public Collider2D bCollider;
    [HideInInspector]
    public bool inputEnabled = false;
    [HideInInspector]
    public Rigidbody2D rb2D;
    public bool canChangePawn = true;
    
    public State state;
    
    public bool reset;
    bool playingCoroutine;

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
        defaultCam = mainCam;
        StartCoroutine(Blink());
    }

    void Update()
    {
        if(state != State.Platform)
            if(sprite.flipX)
                autoLook.lightsLeft();
            else
                autoLook.lightsRight();

        switch (state)
        {
            case State.Auto:
                //inputEnabled = false;
                defaultCam = mainCam;
                autoLook.isOn = true;
                break;

            case State.Rage:
               
                break;

            case State.Controlled:
                //inputEnabled = true;
                autoLook.isOn = false;
                movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                if (((movement.x < 0 && !sprite.flipX) || (movement.x > 0 && sprite.flipX)))
                    sprite.flipX = !sprite.flipX;
                if (inputEnabled && Input.GetButtonDown("ChangePawn") && canChangePawn)
                {
                    mainScript.ChangePawn(1);
                    state = State.Auto;
                    EnableFollowing();
                }
                if (inputEnabled && Input.GetButtonDown("Fire1") && canPlatform)
                {
                    AudioManager.instance.PlaySound(sounds[0], transform.position, 0.5f);
                    inputEnabled = false;
                    StartCoroutine(WaitForIn());
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
                if ((inputEnabled && (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical"))) || reset)
                {
                    AudioManager.instance.PlaySound(sounds[1], transform.position, 0.3f);
                    animator.SetBool("isPlatform", false);
                    inputEnabled = false;
                    
                    StartCoroutine(WaitForAnim());
                }
                if (inputEnabled && Input.GetButtonDown("ChangePawn"))
                {
                    mainScript.ChangePawn(1);
                    inputEnabled = false;
                }
                if (Vector3.Distance(transform.position, mainScript.playerPawn.transform.position) > distanceToReset && !playingCoroutine)
                {
                    AudioManager.instance.PlaySound(sounds[1], transform.position, 0.5f);
                    reset = true;
                    inputEnabled = false;
                    bCollider.enabled = false;
                    circleCollider.enabled = false;
                }
                    
                break;
        }

        
    }

    private void FixedUpdate()
    {
        if (state == State.Controlled) 
            rb2D.AddForce(Vector2.ClampMagnitude(movement * speed * 10, 40));
        
    }

    public void EnableRage()
    {
        Debug.Log("Rage");
        state = State.Rage;
        animator.enabled = false;
        autoLook.isOn = true;
        autoLook.target = FindObjectOfType<Zorg>().gameObject;
        sprite.sprite = rageSprite;
        lights[0].color = Color.red;
        lights[1].color = Color.red;

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
        float animLenght = animator.GetCurrentAnimatorStateInfo(0).length;
        playingCoroutine = true;
        bool wasReset = false;
        if (reset)
            wasReset = true;
        reset = false;
        float t = 0;
        while (t < animLenght)
        {
            t += Time.deltaTime;
            yield return null;
        }
        yield return new WaitUntil(() => t >= animLenght);
        for (int i = 0; i < lights.Length; i++)
            lights[i].color = defaultColor;
        bCollider.enabled = false;
        playingCoroutine = false;
        if (wasReset && mainScript.currentPawn == Main.CurrentPawn.Carrie)
        {
            wasReset = false;
            rb2D.bodyType = RigidbodyType2D.Dynamic;
            mainScript.ChangePawn(1);
            inputEnabled = false;
            state = State.Auto;
            EnableFollowing();
        }
        else
        {
            inputEnabled = true;
            state = State.Controlled;
            rb2D.bodyType = RigidbodyType2D.Dynamic;
        }       
    }

    IEnumerator WaitForIn()
    {
        float t = 0;
        while (t < animator.GetCurrentAnimatorStateInfo(0).length)
        {
            t += Time.deltaTime;
            yield return null;
        }
        yield return new WaitUntil(() => t >= animator.GetCurrentAnimatorStateInfo(0).length);
        inputEnabled = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, distanceToReset);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, distanceLimit);
    }

    public void ChangeCam()
    {
        if (mainCam.activeSelf && !closeCam.activeSelf)
        {
            closeCam.SetActive(true);
            mainCam.SetActive(false);
            defaultCam = closeCam;
        }
        else if (closeCam.activeSelf && !mainCam.activeSelf)
        {
            mainCam.SetActive(true);
            closeCam.SetActive(false);
            defaultCam = mainCam;
        }
            
    }
}
