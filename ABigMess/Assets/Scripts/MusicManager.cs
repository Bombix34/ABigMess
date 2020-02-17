using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : Singleton<MusicManager> 
{
    Radio radioSystem;

    private SoundManager soundManager;

    private void Start()
    {
        soundManager = this.GetComponent<SoundManager>();
    }

    /// <summary>
    /// Function to start and switch radio station
    /// </summary>
    public void SwitchRadio()
    {
        if (radioSystem == null)
        {
            radioSystem = new Radio(this.gameObject);
        }
        else if(!radioSystem.IsRadioLaunch())
        {
            radioSystem.LaunchRadio();
        }
        radioSystem.SwitchRadioChannel();
    }

    public void ShutRadio()
    {
        if(radioSystem!=null)
        {
            radioSystem.StopRadio();
        }
    }

    public void StressChronoSound()
    {
        //Debug.Log("BIP");
        //APPEL DE LEVENT STRESS SOUND
    }

    public void MainMenuMusic(bool isOn)
    {
        AkSoundEngine.PostEvent(isOn ? "Play_Music_menu" : "Stop_Music_menu", gameObject);
    }

    public void SwitchStateMusicNoon(int layer)
    {
        if(layer==0)
        {
            AkSoundEngine.PostEvent("Play_music_noon", gameObject);
        }
        else if(layer<9)
        {
            AkSoundEngine.PostEvent("Set_state_noon_0" + layer,gameObject);
        }
        else
        {
            AkSoundEngine.PostEvent("Set_state_noon_08", gameObject);
        }
    }

    public void SwitchStateMusicMorning(int layer)
    {
        if (layer == 0)
        {
            AkSoundEngine.PostEvent("Play_music_morning", gameObject);
        }
        else if (layer < 6)
        {
            AkSoundEngine.PostEvent("Set_state_music_morning_0" + layer, gameObject);
        }
        else
        {
            AkSoundEngine.PostEvent("Set_state_music_morning_05", gameObject);
        }
    }

    public void TransitionLevel(bool isIn)
    {
        if (isIn)
        {
            AkSoundEngine.PostEvent("Play_transition_sound_in", gameObject);
            AkSoundEngine.PostEvent("Transition_level_in", gameObject);
        }
        else
        {
            AkSoundEngine.PostEvent("Play_transition_sound_out", gameObject);
            AkSoundEngine.PostEvent("Transition_level_out", gameObject);
        }
    }

    public void CreditMusic()
    {
        AkSoundEngine.PostEvent("Play_Music_credits", gameObject);
    }

    #region PRESENTATION

    //ATTENTION : UNIQUEMENT POUR LES NVIEAUX PRESENTATIONS
    public bool isMusicLaunch = false;

    public void LaunchMusic()
    {
        AkSoundEngine.PostEvent("Play_music_noon", gameObject);
    }

    public void StopMusic()
    {
        if (!isMusicLaunch)
        {
            return;
        }
        isMusicLaunch = false;
        AkSoundEngine.PostEvent("Transition_level_in", gameObject);
        AkSoundEngine.PostEvent("Stop_music_noon", gameObject);
        AkSoundEngine.PostEvent("Stop_music_morning", gameObject);

    }

    public void ForceSwitchStateMusic(int indexLayer)
    {
        if(!isMusicLaunch)
        {
            LaunchMusic();
            isMusicLaunch = true;
        }
        AkSoundEngine.PostEvent("Transition_level_out", gameObject);
        AkSoundEngine.PostEvent("Set_state_noon_0" + (indexLayer), gameObject);
    }

    public void PlayTrailerSound()
    {
        AkSoundEngine.PostEvent("Play_trailer_bigmess_audio", gameObject);
    }

    public void StopTrailerSound()
    {
        AkSoundEngine.PostEvent("Stop_trailer_sound", gameObject);
    }

    #endregion  


    public SoundManager GetSoundManager()
    {
        if(soundManager==null)
        {
            soundManager = this.gameObject.AddComponent<SoundManager>();
        }
        return soundManager;
    }

}

#region RadioClass

public class Radio
{
    RadioState currentState=RadioState.Off;
    GameObject akSoundManager;

    public Radio(GameObject obj)
    {
        akSoundManager = obj;
        LaunchRadio();
        
    }

    public bool IsRadioLaunch()
    {
        return currentState != RadioState.Off;
    }

    public void LaunchRadio()
    {
        AkSoundEngine.PostEvent("Play_radio_state", akSoundManager);
    }

    public void SwitchRadioChannel()
    {
        currentState++;
        if (currentState==RadioState.Count)
        {
            currentState = RadioState.First;
        }
        switch(currentState)
        {
            case RadioState.First:
                AkSoundEngine.PostEvent("Play_radio_station01",akSoundManager);
                break;
            case RadioState.Second:
                AkSoundEngine.PostEvent("Play_radio_station02", akSoundManager);
                break;
            case RadioState.Third:
                AkSoundEngine.PostEvent("Play_radio_station03", akSoundManager);
                break;
        }
    }

    public void StopRadio()
    {
        if (currentState == RadioState.Off)
            return;
        currentState = RadioState.Off;
        AkSoundEngine.PostEvent("Stop_radio_stations", akSoundManager);
    }

    public enum RadioState
    {
        Off,
        First,
        Second,
        Third,
        Count
    }

    #region GET/SET
    public RadioState CurrentState
    {
        get => currentState;
        set
        {
            currentState = value;
        }
    }
    #endregion

}
#endregion
