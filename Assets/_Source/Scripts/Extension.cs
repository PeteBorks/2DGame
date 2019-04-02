/**
 * Extension.cs
 * Created by: Pedro Borges
 * Created on: 30/03/19 (dd/mm/yy)
 */

using UnityEngine;
using System.Collections;
using System;

public static class Extensions
{
    public static void SetChildrenLayerRecursevely(this Transform parent, int layer)
    {
        parent.gameObject.layer = layer;

        if (parent.childCount > 0)
        {
            foreach (Transform child in parent)
                child.SetChildrenLayerRecursevely(layer);
        }
    }

    public static Coroutine DelayedCall(this MonoBehaviour monoScript, float delay, Action call)
    {
        return monoScript.StartCoroutine(DelayedCallRoutine(delay, call));
    }

    public static IEnumerator DelayedCallRoutine(float delay, Action call)
    {
        yield return new WaitForSeconds(delay);
        call();
    }
}
