/**
 * BreakableWall.cs
 * Created by: Pedro Borges
 * Created on: 26/03/19 (dd/mm/yy)
 */

using UnityEngine;

public class BreakableWall : BreakablePlatform
{
    enum Direction
    {
        Right,
        Left
    }

    [SerializeField, Range(0, 2)]
    float distance;
    [SerializeField]
    Direction direction;
    [SerializeField]
    LayerMask playerLayer;
    public GameObject attachedLevel;
    BoxCollider2D boxCollider2D;
    Vector2 dir;
    ContactFilter2D filter;
    RaycastHit2D[] results;

    void OnValidate()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        switch(direction)
        {
            case Direction.Right: dir = transform.right;
                break;
            case Direction.Left: dir = -transform.right;
                break;
        }
    }

    void Start()
    {
        bcollider2D = GetComponent<BoxCollider2D>();
        OnValidate();
        filter = new ContactFilter2D()
        {
            useLayerMask = false,
            layerMask = playerLayer,
            maxDepth = 10,
            minDepth = -10
        };
    }

    void Update()
    {
        if(boxCollider2D.enabled)
            if (Physics2D.Raycast(transform.position + new Vector3(boxCollider2D.bounds.extents.x * dir.x, 0, 0) * 1.05f, dir, distance) && !coroutineStarted)
            {
                coroutineStarted = true;
                StartCoroutine(BreakBlock(DestroyBlock));
            }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        player = collision.gameObject.GetComponent<PlayerController>();
    }

    public override void DestroyBlock()
    {
        bcollider2D.enabled = false;
        if (player.rightSideCheck)
        {
            player.RightSideDetach();
            player.isFacingRight = false;
        }
        if (player.leftSideCheck)
        {
            player.LeftSideDetach();
            player.isFacingRight = true;
        }
        player.animator.SetBool("isOnAir", true);
        player.animator.SetBool("isGrabbing", false);
        for (int i = 0; i < l.Length; i++)
            l[i].enabled = false;
        StartCoroutine(ReactivateBlock(ReactivateComplete));
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + new Vector3(boxCollider2D.bounds.extents.x * dir.x, 0, 0) * 1.05f, transform.position + (new Vector3(dir.x, dir.y, 0) * distance));
    }
}