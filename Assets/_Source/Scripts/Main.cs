/**
 * Main.cs
 * Created by: Pedro Borges
 * Created on: 20/03/19 (dd/mm/yy)
 */

using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour
{

    public PlayerController playerPawn;
    public MeepController stellar;

    GameObject currentPawn;

    private void Awake()
    {
        stellar = GameObject.FindWithTag("Stellar").GetComponent<MeepController>();
        playerPawn = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    public void DisableInput()
    {
        playerPawn.inputEnabled = false;
    }

    public void EnableInput()
    {
        playerPawn.inputEnabled = true;
    }

    public bool GetInput()
    {
        return playerPawn.inputEnabled;
    }

    public void ChangePawn(float pawnN)
    {
        switch(pawnN)
        {
            case 1:
                StartCoroutine("EnablePlayerPawn");
                break;

            case 2: 
                StartCoroutine("EnableStellar");
                break;
        }
    }

    IEnumerator EnableStellar()
    {
        playerPawn.rb2D.simulated = false;
        playerPawn.rightCam.SetActive(false);
        playerPawn.leftCam.SetActive(false);
        playerPawn.inputEnabled = false;
        stellar.mainCam.SetActive(true);
        stellar.DisableFollowing();
        stellar.collider2D5.enabled = true;
        yield return new WaitForSeconds(1);
        stellar.inputEnabled = true;
    }

    IEnumerator EnablePlayerPawn()
    {
        //stellar.rb2D.simulated = false;
        stellar.mainCam.SetActive(false);
        stellar.inputEnabled = false;
        stellar.collider2D5.enabled = false;
        if(!playerPawn.animator.GetBool("isGrabbing"))
            playerPawn.rb2D.simulated = true;
        if (playerPawn.isFacingRight)
            playerPawn.rightCam.SetActive(true);
        else
            playerPawn.leftCam.SetActive(true);
        yield return new WaitForSeconds(1);
        playerPawn.inputEnabled = true;
        //stellar.rb2D.simulated = true;
        stellar.EnableFollowing();
    }
}
