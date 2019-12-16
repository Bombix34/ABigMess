using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    MusicManager musicManager;

    [SerializeField]
    List<PlayerManager> players;
    
    [SerializeField]
    GameObject middlePlayers;       //Position between players for cameras

    private void Awake()
    {
        musicManager = GetComponent<MusicManager>();
    }

    private void Start()
    {
        Application.targetFrameRate = 30;
        QualitySettings.vSyncCount = 0;
    }

    private void Update()
    {
        middlePlayers.transform.position = GetPositionBetweenPlayers();
    }

    #region PLAYERS_GESTION

    public bool PlayerInSameRoom()
    {
        if (players.Count == 1)
        {
            return true;
        }
        int firstPlayerRoom = players[0].CurrentRoomNb;
        for(int i =1; i < players.Count;i++)
        {
            if (firstPlayerRoom != players[i].CurrentRoomNb)
                return false;
        }
        return true;
    }

    public List<PlayerManager> GetPlayers()
    {
        return players;
    }

    public Vector3 GetPositionBetweenPlayers()
    {
        Vector3 result = new Vector3(0, 0, 0);
        foreach(var item in players)
        {
            result += item.transform.position;
        }
        result /= players.Count;
        return result;
    }

    public float GetPositionBetweenPlayersAmplitude()
    {
        Vector3 player1Pos = players[0].transform.position;
        Vector3 middlePos = middlePlayers.transform.position;
        Vector3 resultVector = middlePos - player1Pos;
        return resultVector.magnitude;
    }

    #endregion

    #region GET/SET
    public MusicManager MusicManager
    {
        get => musicManager;
        set { MusicManager = value; }
    }
    #endregion
}
