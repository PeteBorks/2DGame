/**
 * GetPlayerRef.cs
 * Created by: Pedro Borges
 * Created on: 17/04/19 (dd/mm/yy)
 */

using UnityEngine;

public class GetPlayerRef : MonoBehaviour
{
    public PlayerController player;
    public MeepController stellar;
    public Main main;
    void Start()
    {
        main = FindObjectOfType<Main>();
        player = FindObjectOfType<PlayerController>();
        stellar = FindObjectOfType<MeepController>();
    }

    public void EnableInput()
    {
        switch (main.currentPawn)
        {
            case Main.CurrentPawn.Stellar:
                stellar.inputEnabled = true;
                break;
            case Main.CurrentPawn.Carrie:
                player.inputEnabled = true;
                break;
        }
    }

    public void DisableInput()
    {
        stellar.inputEnabled = false;
        player.inputEnabled = false;
    }

    public void ChangePawn()
    {
        switch (main.currentPawn)
        {
            case Main.CurrentPawn.Stellar:
                main.ChangePawn(1);
                    stellar.state = MeepController.State.Auto;
                    stellar.EnableFollowing();
                break;
            case Main.CurrentPawn.Carrie:
                main.ChangePawn(2);
                break;
        }
    }
         
}