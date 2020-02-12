using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentationInputsManager : MonoBehaviour
{
    public KeyCode loadNextSceneInput;
    public KeyCode loadPreviousSceneInput;
    public KeyCode reloadSceneKey;

    public bool ReloadSceneInput()
    {
        return Input.GetKeyDown(reloadSceneKey);
    }

    public bool NextSceneInput()
    {
        return Input.GetKeyDown(loadNextSceneInput);
    }

    public bool PreviousSceneInput()
    {
        return Input.GetKeyDown(loadPreviousSceneInput);
    }

    public bool LoadLevelInput(KeyCode key)
    {
        return Input.GetKeyDown(key);
    }
}
