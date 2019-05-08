/**
 * ButtonActionTargetInfo.cs
 * Created by: Pedro Borges
 * Created on: 26/03/19 (dd/mm/yy)
 */

using UnityEngine;

[System.Serializable]
public class ButtonActionTargetInfo
{
    public string name;
    public Transform buttonTarget;
    public Vector3 target;
    //[HideInInspector]
    public Vector3 origin;
    public bool useDifSprite;
}