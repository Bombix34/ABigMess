using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    Radio radioSystem;

    void Awake()
    {
        AkSoundEngine.PostEvent("Play_musique_test", gameObject);
        AkSoundEngine.SetState("MUTE", "down");
    }

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

}

public class Radio
{
    RadioState currentState=RadioState.Off;
    GameObject akSoundManager;

    public RadioState CurrentState
    {
        get=>currentState;
        set
        {
            currentState = value;
        }
    }

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

    public void StopRadio(GameObject obj)
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
}
