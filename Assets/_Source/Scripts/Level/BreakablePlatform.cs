/**
 * BreakablePlatform.cs
 * Created by: Pedro Borges
 * Created on: 26/03/19 (dd/mm/yy)
 */

using UnityEngine;
using System.Collections;

public class BreakablePlatform : MonoBehaviour
{
    public SpriteRenderer sprite;
    public BoxCollider2D bcollider2D;

    [SerializeField, Range(0, 3)]
    float breakDelay = 1;
    [SerializeField, Range(0, 5)]
    float reactivateTime = 3;
    public AnimationCurve transitionCurve;
    Light [] lights;
    float [] defaultIntensity;
    [HideInInspector]
    public bool coroutineStarted;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        bcollider2D = GetComponent<BoxCollider2D>();
        lights = GetComponentsInChildren<Light>();
            if(lights[0])
        defaultIntensity[0] = lights[0].intensity;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerController>())
            StartCoroutine(BreakBlock(DestroyBlock));
    }

    public IEnumerator BreakBlock(System.Action onComplete = null)
    {
        animator.SetBool("isBreaking", true);
        float time = 0;
        while(time < animator.GetCurrentAnimatorStateInfo(0).length)
        {
            time += Time.deltaTime;
            
            yield return null;
        }
        onComplete?.Invoke();
    }

    public virtual void DestroyBlock()
    {
        animator.SetBool("isBreaking", false);
        bcollider2D.enabled = false;
        
        StartCoroutine(ReactivateBlock(ReactivateComplete));
    }

    public IEnumerator ReactivateBlock(System.Action onComplete = null)
    {
        animator.SetBool("isRecovering", true);
        foreach(Light l in lights)
        {
            l.color = Color.red;
        }
        float time2 = 0;

        while (time2 < animator.GetCurrentAnimatorStateInfo(0).length) 
        {
            time2 += Time.deltaTime;
    
            yield return null;
        }
        onComplete?.Invoke();
    }
    
    public void ReactivateComplete()
    {
        animator.SetBool("isRecovering", false);
        foreach (Light l in lights)
        {
            l.color = Color.green;
        }
        bcollider2D.enabled = true;
        coroutineStarted = false;
    }
    void OnValidate()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
}