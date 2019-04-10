/**
 * SwitchCamera.cs
 * Created by: Pedro Borges
 * Created on: 10/04/19 (dd/mm/yy)
 */

using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
	public GameObject DRightVCam;
	public GameObject DLeftVCam;
    GameObject defaultRCam;
    GameObject defaultLCam;

    void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.CompareTag("Player"))
		{
            
			PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            defaultRCam = player.rightCam;
            defaultLCam = player.leftCam;
            player.rightCam.SetActive(false);
            player.leftCam.SetActive(false);
            player.rightCam = DRightVCam;
			player.leftCam = DLeftVCam;
            if(player.isFacingRight)
            {
                Debug.Log("r");
                player.rightCam.SetActive(true);
            }
            else
            {
                Debug.Log("l");
                player.rightCam.SetActive(false);
            }
            player.currentCam.enabled = true;

		}
	}

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.rightCam.SetActive(false);
            player.leftCam.SetActive(false);
            player.rightCam = defaultRCam;
            player.leftCam = defaultLCam;
            if (player.isFacingRight)
            {
                player.rightCam.SetActive(true);
            }
            else
            {
                player.rightCam.SetActive(false);
            }
        }
    }

}