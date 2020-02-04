using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{

    [SerializeField]
    private MetricsManager metricsManager;

    public MusicManager MusicManager { get; set;  }
    public SceneObjectDatas ObjectsDatas { get; set; }
    private ObjectTasksManager taskManager;

    [SerializeField]
    private LevelDatabase levels;

    [SerializeField]
    private float currentPlayersTime=0f;

    [SerializeField]
    List<PlayerManager> players;
    
    [SerializeField]
    GameObject middlePlayers;       //Position between players for cameras

    private UIManager uiManager;

    private bool isLaunch = false;

    protected override void Awake()
    {
        base.Awake();
        ObjectsDatas = GetComponent<SceneObjectDatas>();
        taskManager = GetComponent<ObjectTasksManager>();
    }

    private void Start()
    {
        uiManager = UIManager.Instance;
        uiManager.IntroScreenTransition();
        MusicManager = MusicManager.Instance;
        MusicManager.SwitchStateMusicNoon();
        for (int i = 0; i < levels.levels.Count; ++i)
        {
            if (levels.levels[i].sceneToLoad == SceneManager.GetActiveScene().name)
            {
                levels.CurrentLevelIndex = i;
            }
        }
        print(levels.CurrentLevelIndex);
        InvokeRepeating("DetectStressTime", 1f, 1f);

        Application.targetFrameRate = 50;
        QualitySettings.vSyncCount = 0;
    }

    private void Update()
    {
        middlePlayers.transform.position = GetPositionBetweenPlayers();
        DebugDisplayInput();
        if(isLaunch)
        {
            currentPlayersTime += Time.deltaTime;
            uiManager.UpdateChronoUI(GetCurrentTime()[0],GetCurrentTime()[1]);
        }
    }

    public void WinCurrentLevel()
    {
        levels.CurrentLevel.PlayerTime = levels.CurrentLevel.startChrono - currentPlayersTime;
        if(metricsManager.saveMetrics)
        {
            string playerTime= GetCurrentTime()[0] + ":" + GetCurrentTime()[1];
            metricsManager.AddLine(levels.CurrentLevel.sceneToLoad, playerTime);
            if(levels.IsFinalLevel())
            {
                StartCoroutine(metricsManager.CreateArchiveCSV("METRICS_" + System.DateTime.Now.ToString("yyyy MMMM")));
            }
        }
        StartCoroutine(LoadNewLevel());
    }

    /// <summary>
    /// when the transition screen UI is ended
    /// we start chrono, music etc
    /// </summary>
    public void LaunchLevel()
    {
        MusicManager.TransitionLevel(false);
        isLaunch = true;
    }

    private IEnumerator LoadNewLevel()
    {
        yield return new WaitForSeconds(0.75f);
        uiManager.EndLevelTransition();
        MusicManager.ShutRadio();
        yield return new WaitForSeconds(1f);
        levels.LoadNextLevel();
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void DebugDisplayInput()
    {
        foreach(PlayerManager player in players)
        {
            if(player.Inputs.GetStartInput())
            {
                GetComponent<DebugDisplay>().IsShowingDebug = !GetComponent<DebugDisplay>().IsShowingDebug;
            }
        }
    }

    private void DetectStressTime()
    {
        if(levels.CurrentLevel.startChrono - currentPlayersTime <= 20f)
        {
            MusicManager.StressChronoSound();
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


    public ObjectTasksManager TasksManager
    {
        get => taskManager;
        set
        {
            taskManager = value;
        }
    }

    public bool PlayersPressValidateInput()
    {
        bool result = false;
        foreach (PlayerManager player in players)
        {
            if (player.Inputs.GetGrabInput())
            {
                result = true;
            }
        }
        return result;
    }

    public List<int> GetCurrentTime()
    {
        float currentTime = levels.CurrentLevel.startChrono - currentPlayersTime;
        if (currentTime < 0)
        {
            currentTime = 0f;
        }
        int minutes = (int)(currentTime / 60f);
        int seconds = (int)(currentTime % 60f);
        List<int> toReturn = new List<int>();
        toReturn.Add(minutes);
        toReturn.Add(seconds);
        return toReturn;
    }

    public Level GetCurrentLevel()
    {
        return levels.GetCurrentLevel();
    }

    public LevelDatabase Levels
    {
        get => levels;
    }

}
