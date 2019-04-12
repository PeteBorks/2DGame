/**
 * DamageableRay.cs
 * Created by: Pedro Borges
 * Created on: 11/04/19 (dd/mm/yy)
 */

using UnityEngine;

public class DamageableRay : MonoBehaviour
{
		void OnTriggerEnter2D(Collider2D collision)
		{
				collision.GetComponent<PlayerController>().TakeDamage(10);
		}
}