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
    public bool runIdleAnim = true;
    public float laserStartInterval;
    public float laserInterval;
    public float laserSecondInterval;
    public float damage;

    private void Start()
    {
        if (!alwaysOn)
            RoutineHelper.StartRoutine(OnOff());
        if (runIdleAnim && GetComponent<Animator>())
            GetComponent<Animator>().SetTrigger("idle");
    }

    void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.GetComponent<PlayerController>())
			collision.GetComponent<PlayerController>().TakeDamage(damage);
        if (collision.GetComponent<MeepController>() && collision.GetComponent<MeepController>().state == MeepController.State.Controlled && damage > 0)
        {
            collision.GetComponent<MeepController>().mainScript.ChangePawn(1);
            collision.GetComponent<MeepController>().state = MeepController.State.Auto;
            collision.GetComponent<MeepController>().EnableFollowing();
        }
            
	}

    IEnumerator OnOff()
    {
        while (true)
        {
            yield return new WaitForSeconds(laserStartInterval);
            if (gameObject)
                gameObject.SetActive(false);
            else
                yield return null;
            yield return new WaitForSeconds(laserInterval);
            gameObject.SetActive(true);
            GetComponent<Animator>().enabled = false;
            yield return new WaitForSeconds(laserSecondInterval);
        }
    }
}