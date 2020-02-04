using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{

    public string nextScene = "level01";

    private List<PlayerInputManager> inputManagers;

    [SerializeField]
    private LevelDatabase levels;

    private void Start()
    {
        inputManagers = new List<PlayerInputManager>();
        levels.ResetPlayersTime();
        PlayerInputManager[] manager = this.GetComponents<PlayerInputManager>();
        for(int i = 0; i < manager.Length;++i)
        {
            inputManagers.Add(manager[i]);
        }
    }

    void Update()
    {
        foreach(var inputPlayer in inputManagers)
        {
            if(inputPlayer.GetPressAnyButtonDown())
            {
                StartCoroutine(LoadFirstLevel());
            }
        }
    }

    IEnumerator LoadFirstLevel()
    {
        yield return new WaitForSeconds(0.2f);
        levels.CurrentLevelIndex = 0;
        SceneManager.LoadScene(nextScene);
    }
}
