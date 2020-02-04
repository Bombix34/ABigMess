using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : Singleton<MusicManager> 
{
    Radio radioSystem;
    
    void Start()
    {
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

    public void SwitchStateMusicNoon()
    {
        int curLevelIndex = GameManager.instance.Levels.CurrentLevelIndex;
        if(curLevelIndex==0)
        {
            AkSoundEngine.PostEvent("Play_music_noon", gameObject);
        }
        else if(curLevelIndex<7)
        {
            AkSoundEngine.PostEvent("Set_state_noon_0" + (curLevelIndex + 1),gameObject);
        }
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
