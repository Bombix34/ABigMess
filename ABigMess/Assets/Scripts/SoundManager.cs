using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public void QuackSound(int player)
    {
        AkSoundEngine.PostEvent("Play_Voice_player0" + player.ToString(), gameObject);
    }

    public void PlayHeavyBringObject()
    {
        AkSoundEngine.PostEvent("Play_Alien_heavy_object", gameObject);
    }

    public void PlayHveayBringLoop(bool isOn)
    {
        AkSoundEngine.PostEvent(isOn ? "Play_voices_carry_loop":"Stop_voice_carry_loop",gameObject);
    }

    public void PlayGrabObjectSound()
    {
        AkSoundEngine.PostEvent("Play_grab_SFX", gameObject);
    }

    public void PlayReleaseObjectSound()
    {
        AkSoundEngine.PostEvent("Play_release_SFX", gameObject);
    }

    public void UseSpongeSound()
    {
        AkSoundEngine.PostEvent("Play_wash_SFX", gameObject);
    }

    public void BurnSFX()
    {
        AkSoundEngine.PostEvent("Play_burn_SFX", gameObject);
    }

    public void CookSFX()
    {
        AkSoundEngine.PostEvent("Play_cook_SFX", gameObject);
    }

    public void PlayWinSFX()
    {
        AkSoundEngine.PostEvent("Play_confettis_SFX", gameObject);
    }

    public void PaintSFX()
    {
        AkSoundEngine.PostEvent("Play_paint_SFX", gameObject);
    }

    public void PlugSFX(bool isPlug)
    {
        if(isPlug)
        {
            AkSoundEngine.PostEvent("Play_device_plug_SFX", gameObject);
        }
        else
        {
            AkSoundEngine.PostEvent("Play_device_unplug_SFX", gameObject);
        }
    }
}
