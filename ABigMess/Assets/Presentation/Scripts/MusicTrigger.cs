using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    private MusicManager musicManager;
    public int musicLayerToLoad=1;


    private void Start()
    {
        musicManager = MusicManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(musicManager==null)
        {
            return;
        }
        if(other.CompareTag("Player"))
        {
            musicManager.ForceSwitchStateMusic(musicLayerToLoad);
        }
    }
}
