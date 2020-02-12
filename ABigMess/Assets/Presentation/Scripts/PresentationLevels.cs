using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "BIGMESS_PRESENTATION/scenes datas")]
public class PresentationLevels :ScriptableObject
{
    public List<Object> scenesList;
    public int curIndexScene = 0;

    public void ReloadScene()
    {
        SceneManager.LoadScene(scenesList[curIndexScene].name);
    }

    public void LoadNextScene()
    {
        IncrementSceneIndex(true);
        SceneManager.LoadScene(scenesList[curIndexScene].name);
    }

    public void LoadPreviousScene()
    {
        IncrementSceneIndex(false);
        SceneManager.LoadScene(scenesList[curIndexScene].name);
    }

    public void IncrementSceneIndex(bool isPlus)
    {
        curIndexScene = isPlus ? (curIndexScene == scenesList.Count-1 ? curIndexScene : curIndexScene + 1) : (curIndexScene <= 0 ? 0 : curIndexScene - 1); 
    }

}
