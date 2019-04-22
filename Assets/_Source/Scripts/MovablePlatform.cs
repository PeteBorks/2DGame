/**
 * MovablePlatform.cs
 * Created by: Pedro Borges
 * Created on: 26/03/19 (dd/mm/yy)
 */

using UnityEngine;

public class MovablePlatform : MonoBehaviour
{
    [SerializeField]
    bool isActive = false;
    [SerializeField]
    bool usePlayerContact = true;
    bool canMove = false;
    float time = 0;
    float t = 0;
    BoxCollider2D bcollider2d;
    public float duration;
    public float pause;
    public Vector3 origin;
    public Vector3 target;
    public Light [] l;
    Transform prevtrans;

    void Start()
    {
        bcollider2d = GetComponent<BoxCollider2D>();
    }

    [ContextMenu("Activate")]
    public void Activate()
    {
        isActive = true;
        for(int i = 0; i < l.Length; i++)
                l[i].enabled = true;
    }
    public void Deactivate()
    {
        isActive = false;
        for (int i = 0; i < l.Length; i++)
            l[i].enabled = false;
    }


    void FixedUpdate()
    {
        if (isActive && canMove)
        {
            time += Time.deltaTime;
            t = Mathf.PingPong(time, duration) / duration;
            transform.localPosition = Vector3.Lerp(origin, target, t);
        }
        if ((Mathf.PingPong(time, duration) / duration > 0.999f || Mathf.PingPong(time, duration) / duration < 0.001f) && (isActive || canMove))
        {
            if(usePlayerContact)
                isActive = false;
            Invoke("Activate", pause);
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(usePlayerContact)
        {
            if(collision.gameObject.GetComponent<PlayerController>())
            {
                Activate();
                prevtrans = collision.collider.transform.parent;
                canMove = true;
                collision.collider.transform.SetParent(transform);
            }
        }  
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (usePlayerContact)
        {
            if (collision.gameObject.GetComponent<PlayerController>())
            {
                Deactivate();
                canMove = false;
                collision.collider.transform.SetParent(prevtrans);
            }
        }
    }

    void OnValidate()
    {
        origin = transform.localPosition;
    }
}