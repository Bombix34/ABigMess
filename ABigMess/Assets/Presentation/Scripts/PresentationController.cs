using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PresentationController : Singleton<PresentationController>
{
    public string nextScene;
    public string previousScene;

    public PresentationTransition transitionManager;

    private PresentationInputsManager inputs;

    public bool isFirstScene = false;

    private void Start()
    {
        inputs = GetComponent<PresentationInputsManager>();
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
        MusicManager.instance.ShutRadio();
        if (isNext)
        {
            if(nextScene!=null)
            {
                SceneManager.LoadScene(nextScene);
            }
        }
        else
        {
            if (previousScene != null)
            {
                SceneManager.LoadScene(previousScene);
            }
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
