/**
 * ZoomOut.cs
 * Created by: Pedro Borges
 * Created on: 12/04/19 (dd/mm/yy)
 */

using System.Collections;
using Cinemachine;
using UnityEngine;

public class ZoomOut : MonoBehaviour
{
    GameObject defaultR;
    GameObject defaultL;

    void OnTriggerEnter2D(Collider2D collision)
	{
        PlayerController player = collision.GetComponent<PlayerController>();
        if(player)
        {
            defaultR = player.rightCam;
            defaultL = player.leftCam;
            player.middleCam.SetActive(true);
            player.rightCam.SetActive(false);
            player.leftCam.SetActive(false);
            player.rightCam = player.leftCam = player.middleCam;
		}
	}

    void OnTriggerExit2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player)
        {
            player.rightCam = defaultR;
            player.leftCam = defaultL;
            if (player.isFacingRight)
                player.rightCam.SetActive(true);
            else
                player.leftCam.SetActive(true);
            player.middleCam.SetActive(false);
        }
    }
}