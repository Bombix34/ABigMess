using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public void QuackSound(int player)
    {
        AkSoundEngine.PostEvent("Play_Voice_player0" + player.ToString(), gameObject);
    }

    public void PlayHeavyBringObject(bool isOn)
    {
        if(isOn)
        {
            AkSoundEngine.PostEvent("Play_Alien_heavy_object", gameObject);
        }
    }

    public void PlayGrabObjectSound()
    {
        AkSoundEngine.PostEvent("Play_grab_SFX", gameObject);
    }

    public void PlayReleaseObjectSound()
    {
        AkSoundEngine.PostEvent("Play_release_SFX", gameObject);
    }
}
