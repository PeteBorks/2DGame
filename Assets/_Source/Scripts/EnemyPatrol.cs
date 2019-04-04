/**
 * EnemyPatrol.cs
 * Created by: Pedro Borges
 * Created on: 29/03/19 (dd/mm/yy)
 */

using UnityEngine;
using System.Collections;

[SelectionBase]
public class EnemyPatrol : BaseEntity
{
    enum EnemyState
    {
        Patrol,
        Guard,
        Attack
    }

    [Header("Properties")]
    [SerializeField]
    EnemyState defaultState;
    [SerializeField]
    float fireCooldown;
    [SerializeField]
    float detectionDistance;
    [SerializeField]
    bool flipSide;
    [Header ("References")]
    [SerializeField]
    GameObject bullet;
    [SerializeField]
    GameObject barrelFX;
    [SerializeField]
    Transform gunBarrel;

    [HideInInspector]
    public bool isFacingRight = true;
    public bool isCoroutineStarted;
    bool canBeHit = true;
    EnemyState state;
    SpriteRenderer sprite;
    public Animator animator;
    GameObject target;
    Ray detectionRay;
    RaycastHit2D hit;
    Light headLight;

    void Start()
    {
        state = defaultState;
        sprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        headLight = GetComponentInChildren<Light>();

    }

    void Update()
    {
        detectionRay.origin = gunBarrel.position;
        detectionRay.direction = detectionRay.direction = isFacingRight ? gunBarrel.right : -gunBarrel.right;
        hit = Physics2D.Raycast(detectionRay.origin, detectionRay.direction, detectionDistance);

        if(health > 0)
            switch (state)
            {
                case EnemyState.Attack:
                    Attack();
                    break;

                case EnemyState.Guard:
                    Guard();
                    break;

                case EnemyState.Patrol:
                    Patrol();
                    break;
            }
    }

    void Attack()
    {
        if (!hit || (hit && !hit.collider.GetComponent<PlayerController>()))
        {
            canBeHit = true;
            state = defaultState;
        }
            
        if (!isCoroutineStarted)
        {
            StartCoroutine("AttackSequence");
        }

    }

    void Guard()
    {
        if (hit && hit.collider.GetComponent<PlayerController>())
        {
            target = hit.collider.gameObject;
            state = EnemyState.Attack;
        }
    }

    void Patrol()
    {
        if (hit && hit.collider.GetComponent<PlayerController>())
        {
            target = hit.collider.gameObject;
            state = EnemyState.Attack;
        }
    }

    public void Fire()
    {
        Instantiate(barrelFX, gunBarrel);
        GameObject shot = Instantiate(bullet, gunBarrel.position, Quaternion.identity);
        if (isFacingRight)
            shot.GetComponent<Bullet>().isRight = true;

    }

    public void DamageFeedback(bool once)
    {
        headLight.enabled = !headLight.enabled;
        if (once)
        {
            this.DelayedCall(0.1f, () => DamageFeedback(false));
            if (canBeHit)
                animator.SetTrigger("hit");
        }
    }

    void FlipSide()
    {
        transform.localScale = new Vector3(-transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);
        isFacingRight = !isFacingRight;
    }

    void OnValidate()
    {
 
        if(flipSide)
        {
            FlipSide();
            flipSide = !flipSide;
        }
            
    }

    void OnDrawGizmos()
    {
        detectionRay.origin = gunBarrel.position;
        detectionRay.direction = isFacingRight ? gunBarrel.right : -gunBarrel.right;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(detectionRay.origin, detectionRay.origin + detectionRay.direction * detectionDistance);
    }

    IEnumerator AttackSequence()
    {
        isCoroutineStarted = true;
        animator.SetTrigger("fire");
        yield return new WaitForSeconds(fireCooldown/8);
        animator.SetTrigger("fire");
        canBeHit = true;
        yield return new WaitForSeconds(fireCooldown);
        canBeHit = false;
        isCoroutineStarted = false;
    }
}