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
        switch (main.currentPawn)
        {
            case Main.CurrentPawn.Stellar:
                stellar.inputEnabled = false;
                break;
            case Main.CurrentPawn.Carrie:
                player.inputEnabled = false;
                break;
        }
    }

    public void ChangePawn()
    {
        switch (main.currentPawn)
        {
            case Main.CurrentPawn.Stellar:
                main.ChangePawn(1);
                break;
            case Main.CurrentPawn.Carrie:
                main.ChangePawn(2);
                break;
        }
    }
         
}