/**
 * TriggerActivate.cs
 * Created by: Pedro Borges
 * Created on: 12/04/19 (dd/mm/yy)
 */

using UnityEngine;

public class TriggerActivate : MonoBehaviour
{
	public GameObject objectToActivate;

	void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.GetComponent<PlayerController>())
			objectToActivate.SetActive(true);
	}
}