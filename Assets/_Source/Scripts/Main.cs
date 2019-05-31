/**
 * Main.cs
 * Created by: Pedro Borges
 * Created on: 20/03/19 (dd/mm/yy)
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public CheckpointComponent currentCheckpoint;
    public LoadingScreen loadingScreen;
    public int lastOpenedScene;
    public Slider healthSlider; 
    

    private void Awake()
    {
        stellar = GameObject.FindWithTag("Stellar").GetComponent<MeepController>();
        playerPawn = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        healthSlider.value = playerPawn.health;
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
                //if(currentPawn != CurrentPawn.Carrie)
                    StartCoroutine(EnablePlayerPawn());
                break;

            case 2: 
                //if(currentPawn != CurrentPawn.Stellar)
                    StartCoroutine(EnableStellar());
                break;
        }
    }

    public void ResetToCheckpoint()
    {
        if (lastOpenedScene > 1)
            loadingScreen.sceneIndex = lastOpenedScene;
        else
            loadingScreen.sceneIndex = SceneManager.GetActiveScene().buildIndex;
        loadingScreen.gameObject.SetActive(true);
        loadingScreen.Initialize();
        playerPawn.health = currentCheckpoint.playerHealth;
        playerPawn.GetComponent<CollectableManager>().collectables = currentCheckpoint.collectables;
        playerPawn.transform.position = currentCheckpoint.loadLocation.position;
        RenderSettings.ambientLight = currentCheckpoint.ambientColor;
    }

    public IEnumerator EnableStellar()
    {
        currentPawn = CurrentPawn.Stellar;
        playerPawn.rb2D.simulated = false;
        playerPawn.rightCam.SetActive(false);
        playerPawn.leftCam.SetActive(false);
        playerPawn.inputEnabled = false;
        stellar.defaultCam.SetActive(true);
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
        stellar.defaultCam.SetActive(false);
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
