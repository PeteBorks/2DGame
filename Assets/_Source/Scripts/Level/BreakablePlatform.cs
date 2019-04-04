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
    public Color defaultColor;
    public Color breakColor = Color.black;
    public AnimationCurve transitionCurve;
    public Light [] l;
    public PlayerController player;
    [HideInInspector]
    public bool coroutineStarted;

    void Start()
    {
        bcollider2D = GetComponent<BoxCollider2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(BreakBlock(DestroyBlock));
        player = collision.gameObject.GetComponent<PlayerController>(); ;
    }

    public IEnumerator BreakBlock(System.Action onComplete = null)
    {
        float time = 0;
        while(time < breakDelay)
        {
            time += Time.deltaTime;
            sprite.color = Color.Lerp(defaultColor, breakColor, transitionCurve.Evaluate(time / breakDelay));
            yield return null;
        }
        onComplete?.Invoke();
    }

    public virtual void DestroyBlock()
    {
        bcollider2D.enabled = false;
        for(int i = 0; i < l.Length; i++)
            l[i].enabled = false;
        StartCoroutine(ReactivateBlock(ReactivateComplete));
    }

    public IEnumerator ReactivateBlock(System.Action onComplete = null)
    {
        float time2 = 0;
        
        while (time2 < reactivateTime)
        {
            time2 += Time.deltaTime;
            sprite.color = Color.Lerp(breakColor, defaultColor, transitionCurve.Evaluate(time2 / reactivateTime));
            yield return null;
        }
        onComplete?.Invoke();
    }
    
    public void ReactivateComplete()
    {
        bcollider2D.enabled = true;
        for (int i = 0; i < l.Length; i++)
            l[i].enabled = true;
        sprite.color = defaultColor;
        coroutineStarted = false;
    }

    void OnValidate()
    {
        sprite = GetComponent<SpriteRenderer>();
        
        defaultColor = sprite.color;
    }
}