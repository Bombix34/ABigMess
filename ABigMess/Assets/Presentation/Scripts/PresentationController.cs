using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentationController : Singleton<PresentationController>
{
    public PresentationLevels levelsDatas;

    public PresentationTransition transitionManager;

    private PresentationInputsManager inputs;

    public bool isFirstScene = false;

    private void Start()
    {
        inputs = GetComponent<PresentationInputsManager>();
        if(isFirstScene)
        {
            levelsDatas.curIndexScene = 0;
        }
        if(transitionManager!=null)
        {
            transitionManager.StartSceneTransition();
        }
    }

    private void Update()
    {
        UpdateInputs();
    }

    private void UpdateInputs()
    {
        if(inputs.ReloadSceneInput())
        {
            StopMusic();
            levelsDatas.ReloadScene();
        }
        if(inputs.NextSceneInput())
        {
            StartCoroutine(LoadScene(true));
        }
        if(inputs.PreviousSceneInput())
        {
            StartCoroutine(LoadScene(false));
        }
    }

    public IEnumerator LoadScene(bool isNext)
    {
        if(transitionManager!=null)
        {
            transitionManager.LeaveSceneTransition();
            yield return new WaitForSeconds(0.75f);
        }
        StopMusic();
        if (isNext)
        {
            levelsDatas.LoadNextScene();
        }
        else
        {
            levelsDatas.LoadPreviousScene();
        }
    }

    public void StopMusic()
    {
        if(MusicManager.instance!=null)
        {
            MusicManager.instance.StopMusic();
        }
    }
}
