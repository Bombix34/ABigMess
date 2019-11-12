using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    List<PlayerManager> players;




    public bool PlayerInSameRoom()
    {
        if (players.Count == 1)
            return true;
        int firstPlayerRoom = players[0].CurrentRoomNb;
        for(int i =1; i < players.Count;i++)
        {
            if (firstPlayerRoom != players[i].CurrentRoomNb)
                return false;
        }
        return true;
    }
}
