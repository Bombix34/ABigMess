using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerInputManager : MonoBehaviour
{
    // The Rewired player id of this character
    public int playerId = 0;

    private Player player; // The Rewired Player

    void Awake()
    {
        // Get the Rewired Player object for this player and keep it for the duration of the character's lifetime
        player = ReInput.players.GetPlayer(playerId);
    }

//MOVEMENT INPUTS__________________________

    public float GetMovementInputX()
    {
        return player.GetAxis("Move Horizontal");
    }

    public float GetMovementInputY()
    {
        return player.GetAxis("Move Vertical");
    }

    public Vector3 GetMovementInput()
    {
        Vector3 move = Vector3.zero;
        move.x = player.GetAxis("Move Horizontal");
        move.z = player.GetAxis("Move Vertical");
        return move;
    }

//BUTTONS INPUTS____________________________

    //Interact
    public bool GetInteractInputDown()
    {
        return player.GetButtonDown("Interact");
    }

    public bool GetInteractInput()
    {
        return player.GetButton("Interact");
    }

    public bool GetInteractInputUp()
    {
        return player.GetButtonUp("Interact");
    }

    //Grab
    public bool GetGrabInputDown()
    {
        return player.GetButtonDown("Grab");
    }

    public bool GetGrabInput()
    {
        return player.GetButton("Grab");
    }

    public bool GetGrabInputUp()
    {
        return player.GetButtonUp("Grab");
    }

    //Switch
    public bool GetSwitchInputDown()
    {
        return player.GetButtonDown("Switch");
    }

    public bool GetSwitchInput()
    {
        return player.GetButton("Switch");
    }

    public bool GetSwitchInputUp()
    {
        return player.GetButtonUp("Switch");
    }

    //Quack
    public bool GetQuackInputDown()
    {
        return player.GetButtonDown("Quack");
    }

    public bool GetQuackInput()
    {
        return player.GetButton("Quack");
    }

    public bool GetQuackInputUp()
    {
        return player.GetButtonUp("Quack");
    }

    //Start
    public bool GetStartInput()
    {
        return player.GetButtonDown("Start");
    }

    //Any
    public bool GetPressAnyButton()
    {
        return player.GetAnyButton();
    }

}
