/**
 * RoutineHelper.cs
 * Created by: Pedro Borges
 * Created on: 13/04/19 (dd/mm/yy)
 */

using UnityEngine;
using System.Collections;

public class RoutineHelper : Singleton<RoutineHelper>
{
    public static Coroutine StartRoutine(IEnumerator routine)
    {
        return Instance.StartCoroutine(routine);
    }

    public static void StopRoutine(Coroutine routine)
    {
        Instance.StopCoroutine(routine);
    }
}