using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[CreateAssetMenu(menuName = "BIGMESS_LEVELS/Levels/new level")]
public class Level : ScriptableObject
{
    [Header("write the name of scene to load")]
    public string sceneToLoad;

    [Header("Starting chrono")]
    public float startChrono;
    [Header("Time at which players lose gold medal")]
    public float silverChrono;
    [Header("Time at which players lose silver medal")]
    public float bronzeChrono;
    [Header("Time at which players lose all medal")]
    public float badChrono;

    private float playerTime;

    public TransitionScreen introScreen;

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void ResetPlayeTime()
    {
        playerTime = 0f;
    }

    public float PlayerTime
    {
        get => playerTime;
        set
        {
            if(value>playerTime)
            {
                playerTime=value;
            }
            else if(value<0f)
            {
                playerTime = 0f;
            }
        }
    }


}
