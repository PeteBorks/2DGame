/**
 * MovableLaser.cs
 * Created by: Pedro Borges
 * Created on: 12/04/19 (dd/mm/yy)
 */

using UnityEngine;

public class MovableLaser : MonoBehaviour
{
	public bool isActive;
	public float duration;
	public Vector2 origin;
	public Vector2 target;
	float time;
	float t;

	void FixedUpdate()
    {
        if (isActive)
        {
            time += Time.deltaTime;
            t = Mathf.PingPong(time, duration) / duration;
            transform.localPosition = Vector3.Lerp(origin, target, t);
        }
    }
}