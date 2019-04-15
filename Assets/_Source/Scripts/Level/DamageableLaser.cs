/**
 * DamageableLaser.cs
 * Created by: Pedro Borges
 * Created on: 11/04/19 (dd/mm/yy)
 */

using System.Collections;
using UnityEngine;

public class DamageableLaser : MonoBehaviour
{
    public bool alwaysOn = true;
    public float laserStartInterval;
    public float laserInterval;
    public float laserSecondInterval;
    public float damage;

    private void Start()
    {
        if (!alwaysOn)
            RoutineHelper.StartRoutine(OnOff());
    }

    void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.GetComponent<PlayerController>())
			collision.GetComponent<PlayerController>().TakeDamage(damage);
	}

    IEnumerator OnOff()
    {
        while (true)
        {
            yield return new WaitForSeconds(laserStartInterval);
            gameObject.SetActive(false);
            yield return new WaitForSeconds(laserInterval);
            gameObject.SetActive(true);
            GetComponent<Animator>().enabled = false;
            yield return new WaitForSeconds(laserSecondInterval);
        }
    }
}