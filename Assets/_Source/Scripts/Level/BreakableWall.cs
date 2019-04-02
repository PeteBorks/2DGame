/**
 * BreakableWall.cs
 * Created by: Pedro Borges
 * Created on: 26/03/19 (dd/mm/yy)
 */

using UnityEngine;

public class BreakableWall : BreakablePlatform
{
    public GameObject attachedLevel;

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
}