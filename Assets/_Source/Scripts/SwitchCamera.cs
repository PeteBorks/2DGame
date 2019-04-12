/**
 * SwitchCamera.cs
 * Created by: Pedro Borges
 * Created on: 10/04/19 (dd/mm/yy)
 */

using UnityEngine;



public class SwitchCamera : MonoBehaviour
{
    public enum CameraModes
    {
        Default,
        Middle,
        Down
    }
	GameObject DRightVCam;
	GameObject DLeftVCam;
    GameObject defaultRCam;
    GameObject defaultLCam;
    GameObject middleCam;
    public CameraModes modes;

    void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.CompareTag("Player"))
		{
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            defaultRCam = player.rightCam;
            defaultLCam = player.leftCam;
            switch(modes)
            {
                case CameraModes.Down:
                    DRightVCam = player.DRightCam;
                    DLeftVCam = player.DLeftCam;
                    player.rightCam = DRightVCam;
                    player.leftCam = DLeftVCam;
                    if(player.isFacingRight)
                    {
                        player.rightCam.SetActive(true);
                    }
                    else
                    {
                        player.leftCam.SetActive(true);

                    }
                    break;
                case CameraModes.Middle:
                    middleCam = player.middleCam;
                    player.rightCam = middleCam;
                    player.leftCam = middleCam;
                    middleCam.SetActive(true);
                    break;
            }
            defaultRCam.SetActive(false);
            defaultLCam.SetActive(false);
		}
	}

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.rightCam = defaultRCam;
            player.leftCam = defaultLCam;

            if (player.isFacingRight)
            {
                player.rightCam.SetActive(true);
            }
            else
            {
                player.leftCam.SetActive(true);
            }

            switch(modes)
            {
                case CameraModes.Middle:
                        middleCam.SetActive(false); 
                        break;
                case CameraModes.Down:
                        DRightVCam.SetActive(false);
                        DLeftVCam.SetActive(false);
                        break;
            }
        }
    }
}