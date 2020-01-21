using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        initWeight = body.mass;
        objectMovement = this.gameObject.AddComponent<PlayerMovement>();
        objectMovement.CanRotateTorso = false;
    }

    void Start()
    {

    }

    void Update()
    {
        SetupMovementInput();
        UpdatePlayersProperty();
            objectMovement.DoMove(playersInput);
            foreach (var player in players)
            {
              player.GetComponent<PlayerManager>().Movement.DoMove(playersInput);

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
                manager.Movement.CanMove = true;
                manager.Movement.CanRotate = false;
                manager.ChangeState(new PlayerHeavyBringState(manager, this.GetComponent<InteractObject>()));
               // player.transform.localScale = Vector3.one;
                player.transform.SetParent(this.transform, true);
            }
        }
    }

    public void EndMovement()
    {
        foreach (var player in players)
        {
            player.GetComponent<PlayerManager>().Movement.CanMove = true;
            player.GetComponent<PlayerManager>().Movement.CanRotate = true;
            player.transform.parent = null;
        }
        Destroy(objectMovement);
        Destroy(this);
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

    public void UpdatePlayers(List<GameObject> curPlayers)
    {
        if(players==null)
        {
            players = new List<GameObject>();
        }
        else
        {
            players.Clear();
        }
        players = curPlayers;
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
