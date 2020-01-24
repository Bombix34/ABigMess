using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    MusicManager musicManager;

    [SerializeField]
    private LevelDatabase levels;

    [SerializeField]
    private float currentPlayersTime=0f;

    [SerializeField]
    List<PlayerManager> players;
    
    [SerializeField]
    GameObject middlePlayers;       //Position between players for cameras

    private UIManager uiManager;

    private void Awake()
    {
        musicManager = GetComponent<MusicManager>();
    }

    private void Start()
    {
        uiManager = UIManager.Instance;
        for (int i = 0; i < levels.levels.Count; ++i)
        {
            if (levels.levels[i].sceneToLoad == SceneManager.GetActiveScene().name)
            {
                levels.CurrentLevelIndex = i;
            }
        }
        InvokeRepeating("DetectStressTime", 1f, 1f);
        Application.targetFrameRate = 50;
        QualitySettings.vSyncCount = 0;
    }

    private void Update()
    {
        middlePlayers.transform.position = GetPositionBetweenPlayers();
        currentPlayersTime += Time.deltaTime;
        //to remove
        //DebugNextLevelInput();
        //_________
        uiManager.UpdateChronoUI(levels.CurrentLevel.startChrono - currentPlayersTime);
    }

    public void WinCurrentLevel()
    {
        levels.CurrentLevel.PlayerTime = levels.CurrentLevel.startChrono - currentPlayersTime;
        levels.LoadNextLevel();
    }

    public void DebugNextLevelInput()
    {
        foreach(PlayerManager player in players)
        {
            if(player.Inputs.GetStartInput())
            {
                WinCurrentLevel();
            }
        }
    }

    private void DetectStressTime()
    {
        if(levels.CurrentLevel.startChrono - currentPlayersTime <= 20f)
        {
            musicManager.StressChronoSound();
        }
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
