using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "BIGMESS/Levels/new database of level")]
public class LevelDatabase : ScriptableObject
{
    [SerializeField]
    private int curLevelIndex = 0;

    [Header("Drag n drop the settings of scenes to load in order")]
    public List<Level> levels;

    public Level GetCurrentLevel()
    {
        return levels[curLevelIndex];
    }

    public void LoadNextLevel()
    {
        curLevelIndex++;
        if(curLevelIndex>=levels.Count)
        {
            curLevelIndex = 0;
        }
        levels[curLevelIndex].LoadScene();
    }

    public void ResetPlayersTime()
    {
        foreach(Level level in levels)
        {
            level.ResetPlayeTime();
        }
    }

    public int CurrentLevelIndex
    {
        get => curLevelIndex;
        set
        {
            curLevelIndex = value;
            if (curLevelIndex >= levels.Count)
            {
                curLevelIndex = 0;
            }
        }
    }

    public bool IsFinalLevel()
    {
        return curLevelIndex == levels.Count - 1;
    }

    public Level CurrentLevel
    {
        get => levels[curLevelIndex];
    }

}
