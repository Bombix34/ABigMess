using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class EndVideoDetection : MonoBehaviour
{
    private VideoPlayer video;

    [SerializeField]
    private SkipScene skipScene;

    private void Awake()
    {
        video = this.GetComponent<VideoPlayer>();    
    }
    
    void Update()
    {
        if(!video.isPlaying && Time.timeSinceLevelLoad>2f)
        {
            skipScene.LoadScene();
        }
    }
}
