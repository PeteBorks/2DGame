/**
 * InputManager.cs
 * Created by: Pedro Borges
 * Created on: 12/04/19 (dd/mm/yy)
 */

using UnityEngine;

public class InputManager : MonoBehaviour
{
    [ReadOnly]
	public float horizontalAxis;
	public bool dash;
	public bool jump;
	public bool fire;
	public bool changePawn;
	public bool interact;

	void Update()
	{
		horizontalAxis = Input.GetAxis("Horizontal");
		fire = Input.GetButtonDown("Fire1") ?  true : false;
		dash = Input.GetButtonDown("Fire3") ?  true : false;
		jump = Input.GetButtonDown("Jump") ?  true : false;
		changePawn = Input.GetButtonDown("ChangePawn") ?  true : false;
		interact = Input.GetButtonDown("Interact") ?  true : false;
	}
}