using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipSceneAuto : MonoBehaviour
{
    public string nextScene;
    public float duration = 0.5f;

    private void Start()
    {
        StartCoroutine(LoadScene());
    }
    private IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(duration);
        SceneManager.LoadScene(nextScene);
    }
}
