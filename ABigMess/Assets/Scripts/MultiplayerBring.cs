﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class MultiplayerBring : MonoBehaviour
{
    PlayerMovement objectMovement;
    List<GameObject> players;

    Vector3 playersInput;

    Rigidbody body;
    float initWeight;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        players = new List<GameObject>();
        initWeight = body.mass;
        objectMovement = this.gameObject.AddComponent<PlayerMovement>();
        objectMovement.CanRotateTorso = false;
        objectMovement.ModificationRotationSpeed = 0.1f;
    }

    void Start()
    {
        body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        SetupMovementInput();
        objectMovement.DoMove(playersInput);
        
        foreach (var player in players)
        {
           // player.GetComponent<PlayerManager>().Movement.DoMove(playersInput);
            PlayerManager manager = player.GetComponent<PlayerManager>();
        }
        
    }

    private void SetupMovementInput()
    {
        playersInput = Vector3.zero;
        foreach(var player in players)
        {
            if(player.GetComponent<PlayerManager>().Inputs.GetMovementInput()==Vector3.zero)
            {
                playersInput = Vector3.zero;
                break;
            }
            playersInput += player.GetComponent<PlayerManager>().Inputs.GetMovementInput();
        }
        playersInput.Normalize();
    }

    private void UpdatePlayersProperty()
    {
        body.mass = players[0].GetComponent<Rigidbody>().mass;
        foreach (var player in players)
        {
            PlayerManager manager = player.GetComponent<PlayerManager>();
            if (manager.GetCurrentState().stateName != "PLAYER_HEAVY_BRING_OBJECT")
            {
                manager.Movement.CanMove = false;
                manager.Movement.CanRotate = false;
                manager.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
                player.transform.SetParent(this.transform, true);
                SetupPlayerConstraint(player);
                manager.ChangeState(new PlayerHeavyBringState(manager, this.GetComponent<InteractObject>()));
               // player.transform.localScale = Vector3.one;
            }
        }
    }

    private void SetupPlayerConstraint(GameObject player)
    {
        PositionConstraint constraintPos = player.GetComponent<PositionConstraint>();
        if (constraintPos==null)
        {
            constraintPos=player.AddComponent<PositionConstraint>();
        }
        ConstraintSource contraintTransform=new ConstraintSource();
        contraintTransform.sourceTransform = this.transform;
        contraintTransform.weight = 1;
        Vector3 posOffset = player.transform.localPosition;
        constraintPos.AddSource(contraintTransform);
        constraintPos.translationOffset = posOffset;
        constraintPos.constraintActive = true;
    }

    /// <summary>
    /// function called when there is 1 players attached to the object
    /// </summary>
    public void EndMovement()
    {
        foreach (var player in players)
        {
            DetachPlayer(player);
        }
        Destroy(objectMovement);
        Destroy(this);
    }

    public void DetachPlayer(GameObject player)
    {
        player.GetComponent<PlayerManager>().Movement.CanMove = true;
        player.GetComponent<PlayerManager>().Movement.CanRotate = true;
        Destroy(player.GetComponent<PositionConstraint>());
        player.transform.parent = null;
    }

    #region GET/SET

    public void UpdatePlayers(GameObject play, bool isAdd)
    {
        if (players == null)
        {
            players = new List<GameObject>();
        }
        if (isAdd)
        {
            players.Add(play);
        }
        else
        {
            players.Remove(play);
        }
        UpdatePlayersProperty();
    }

    public void SetMovementSettings(PlayerReglages reglages)
    {
        if(objectMovement==null)
        {
            objectMovement = this.gameObject.AddComponent<PlayerMovement>();
        }
        objectMovement.Reglages = reglages;
    }
    

    #endregion
}
