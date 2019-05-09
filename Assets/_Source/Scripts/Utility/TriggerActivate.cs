/**
 * TriggerActivate.cs
 * Created by: Pedro Borges
 * Created on: 12/04/19 (dd/mm/yy)
 */

using UnityEngine;

public class TriggerActivate : MonoBehaviour
{
	public GameObject [] objectsToActivate;

	void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.gameObject.GetComponent<PlayerController>())
            foreach (GameObject g in objectsToActivate)
                g.SetActive(true);
	}
}