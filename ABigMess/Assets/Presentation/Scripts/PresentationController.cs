using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PresentationController : Singleton<PresentationController>
{
    //public PresentationLevels levelsDatas;

    public Object nextScene;
    public Object previousScene;

    public PresentationTransition transitionManager;

    private PresentationInputsManager inputs;

    public bool isFirstScene = false;

    private void Start()
    {
        inputs = GetComponent<PresentationInputsManager>();
        Debug.Log(nextScene);
        /*
        if(isFirstScene)
        {
            levelsDatas.curIndexScene = 0;
        }
        */
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
            /*
            levelsDatas.ReloadScene();
            */
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
            if(nextScene!=null)
            {
                SceneManager.LoadScene(nextScene.name);
            }
            //levelsDatas.LoadNextScene();
        }
        else
        {
            if (previousScene != null)
            {
                SceneManager.LoadScene(previousScene.name);
            }
           // levelsDatas.LoadPreviousScene();
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
