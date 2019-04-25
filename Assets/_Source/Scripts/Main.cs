/**
 * Main.cs
 * Created by: Pedro Borges
 * Created on: 20/03/19 (dd/mm/yy)
 */

using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour
{
    public enum CurrentPawn
    {
        Carrie,
        Stellar
    }

    public PlayerController playerPawn;
    public MeepController stellar;
    public CurrentPawn currentPawn;

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
                if(currentPawn != CurrentPawn.Carrie)
                    StartCoroutine(EnablePlayerPawn());
                break;

            case 2: 
                if(currentPawn != CurrentPawn.Stellar)
                    StartCoroutine(EnableStellar());
                break;
        }
    }

    IEnumerator EnableStellar()
    {
        currentPawn = CurrentPawn.Stellar;
        playerPawn.rb2D.simulated = false;
        playerPawn.rightCam.SetActive(false);
        playerPawn.leftCam.SetActive(false);
        playerPawn.inputEnabled = false;
        stellar.mainCam.SetActive(true);
        if(stellar.state == MeepController.State.Auto)
            stellar.DisableFollowing();
        stellar.circleCollider.enabled = true;
        stellar.inputEnabled = true;
        yield return new WaitForSeconds(1);
        if(stellar.state == MeepController.State.Auto)
            stellar.state = MeepController.State.Controlled;
    }

    IEnumerator EnablePlayerPawn()
    {
        
        currentPawn = CurrentPawn.Carrie;
        stellar.mainCam.SetActive(false);
        stellar.inputEnabled = false;
        if(stellar.state==MeepController.State.Controlled)
        {
            stellar.state = MeepController.State.Auto;
            stellar.circleCollider.enabled = false;
            stellar.bCollider.enabled = false;
        }
        //if(!playerPawn.animator.GetBool("isGrabbing"))
        playerPawn.rb2D.simulated = true;
        if (playerPawn.isFacingRight)
            playerPawn.rightCam.SetActive(true);
        else
            playerPawn.leftCam.SetActive(true);
        yield return new WaitForSeconds(1);
        playerPawn.inputEnabled = true;
        if (stellar.state == MeepController.State.Controlled)
            stellar.EnableFollowing();
    }
}
