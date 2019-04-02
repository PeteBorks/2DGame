/**
 * EnemySpriteComponent.cs
 * Created by: Pedro Borges
 * Created on: 29/03/19 (dd/mm/yy)
 */

using UnityEngine;

public class EnemySpriteComponent : MonoBehaviour
{
    EnemyPatrol mainScript;

    void Awake()
    {
        mainScript = GetComponentInParent<EnemyPatrol>();
    }

    public void Fire()
    {
        mainScript.Fire();
    }
}