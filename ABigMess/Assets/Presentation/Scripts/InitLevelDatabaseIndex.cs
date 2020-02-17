using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitLevelDatabaseIndex : MonoBehaviour
{
    private GameManager manager;
    public int indexToSet;

    private void Awake()
    {
        manager = this.GetComponent<GameManager>();
        manager.Levels.CurrentLevelIndex = indexToSet;
    }
    
}
