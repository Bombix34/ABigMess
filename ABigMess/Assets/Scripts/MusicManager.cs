using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    void Start()
    {
        AkSoundEngine.PostEvent("Play_musique_test", gameObject);
        AkSoundEngine.SetState("MUTE", "down");
    }
    
}
