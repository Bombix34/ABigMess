using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

public class MusicSwitchOnStart : MonoBehaviour
{
    private MusicManager manager;
    public UnityEvent onStartEvent;

    private VideoPlayer videoTrailer;

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

    public void TrailerSound()
    {
        if (IsMusicManager)
        {
            manager.isMusicLaunch = true;
            manager.StopMusic();
            this.GetComponent<VideoPlayer>().Play();
            manager.PlayTrailerSound();
            StartCoroutine(WaitForEndTrailer());
        }
    }

    public void EndTrailerSound()
    {
        if (IsMusicManager)
        {
            manager.StopTrailerSound();
        }
    }

    private IEnumerator WaitForEndTrailer()
    {
        yield return new WaitForSeconds(1f);
        while (this.GetComponent<VideoPlayer>().isPlaying)
        {
            yield return new WaitForSeconds(0.001f);
        }
       // EndTrailerSound();
    }
}
