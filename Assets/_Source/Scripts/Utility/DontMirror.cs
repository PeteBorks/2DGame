/**
 * DontMirror.cs
 * Created by: Pedro Borges
 * Created on: 14/04/19 (dd/mm/yy)
 */

using UnityEngine;

public class DontMirror : MonoBehaviour
{
    SpriteRenderer sprite;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (transform.parent.localScale.x > 0)
            sprite.flipX = false;
        else
            sprite.flipX = true;
    }
}