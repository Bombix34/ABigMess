using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SkipScene : MonoBehaviour
{
    private PlayerInputManager[] inputs;

    [SerializeField]
    private GameObject skipImage;

    [SerializeField]
    private PresentationTransition transition;

    bool isSkipButton=false;

    public string nextScene;

    private void Awake()
    {
        inputs = this.GetComponents<PlayerInputManager>();
        skipImage.SetActive(false);
    }

    private void Start()
    {
        if(transition!=null)
        {
            transition.StartSceneTransition();
        }
    }

    private void Update()
    {
        if (!isSkipButton)
        {
            for (int i = 0; i < inputs.Length; ++i)
            {
                if (inputs[i].GetPressAnyButtonDown())
                {
                    skipImage.SetActive(true);
                    isSkipButton = true;
                }
            }
        }
        else
        {
            for (int i = 0; i < inputs.Length; ++i)
            {
                if(inputs[i].GetGrabInputDown())
                {
                    LoadScene();
                }
            }
        }
    }

    public void LoadScene()
    {
        if (transition != null)
        {
            transition.LeaveSceneTransition();
        }
        MusicManager.Instance.ShutRadio();
        MusicManager.Instance.StopMusic();
        SceneManager.LoadScene(nextScene);
    }
}
