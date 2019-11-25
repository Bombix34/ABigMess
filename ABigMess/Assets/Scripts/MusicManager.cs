using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    Radio radioSystem;

    void Start()
    {
        AkSoundEngine.PostEvent("Play_musique_test", gameObject);
        AkSoundEngine.SetState("MUTE", "down");
        radioSystem = new Radio(this.gameObject);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(radioSystem.IsRadioLaunch())
            {
                print(radioSystem.CurrentState);
                radioSystem.SwitchRadioChannel();
            }
            else
            {
                radioSystem.LaunchRadio(this.gameObject);
            }
        }
    }

}

public class Radio
{
    RadioState currentState;
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
        currentState = RadioState.Off;
        AkSoundEngine.PostEvent("Play_radio_state", obj);
        AkSoundEngine.SetState("radio_station", "off");
    }

    public void LaunchRadio(GameObject objAttached)
    {
        AkSoundEngine.PostEvent("Play_radio_state", objAttached);
    }

    public bool IsRadioLaunch()
    {
        return currentState != RadioState.Off;
    }

    public void SwitchRadioChannel()
    {
        if(currentState==RadioState.Third || currentState==RadioState.Off)
        {
            currentState = RadioState.First;
        }
        else
        {
            currentState++;
        }
        switch(currentState)
        {
            case RadioState.First:
                AkSoundEngine.SetState("radio_station", "station01");
                break;
            case RadioState.Second:
                AkSoundEngine.SetState("radio_station", "station02");
                break;
            case RadioState.Third:
                AkSoundEngine.SetState("radio_station", "station03");
                break;
        }
    }

    public enum RadioState
    {
        Off,
        First,
        Second,
        Third
    }
}
