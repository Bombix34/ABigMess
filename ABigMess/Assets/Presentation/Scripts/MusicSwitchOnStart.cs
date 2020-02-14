using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MusicSwitchOnStart : MonoBehaviour
{
    private MusicManager manager;
    public UnityEvent onStartEvent;

    private void Start()
    {
        manager = MusicManager.Instance;    
        if(onStartEvent!=null)
        {
            onStartEvent.Invoke();
        }
    }

    private bool IsMusicManager
    {
        get => manager != null;
    }

    public void MainMenuMusic()
    {
        if(IsMusicManager)
        {
            manager.isMusicLaunch = true;
            manager.StopMusic();
            manager.MainMenuMusic(true);
        }
    }

    public void StopMusic()
    {
        if (IsMusicManager)
        {
            manager.isMusicLaunch = true;
            manager.StopMusic();
        }
    }
}
