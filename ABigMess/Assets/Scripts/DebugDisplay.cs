using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugDisplay : MonoBehaviour
{
    float deltaTime = 0.0f;

    public bool IsShowingDebug { get; set; }

    private void Awake()
    {
        IsShowingDebug = false;
    }

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        if(!IsShowingDebug)
        {
            return;
        }
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        GUIStyle objectsInScene = new GUIStyle();
        Rect rectObj = new Rect(0, (2*h)/3, w, h * 2 / 100);
        objectsInScene.alignment = TextAnchor.UpperLeft;
        objectsInScene.fontSize = h * 2 / 100;
        objectsInScene.normal.textColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        string objText = SceneObjectDatas.Instance.ObjectsToString();
        GUI.Label(rectObj, objText, objectsInScene);

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, style);

    }
}
