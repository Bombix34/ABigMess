using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grown : MonoBehaviour
{
    public Vector3 initialScale;
    public int maxTimesScaled = 5;
    public int timesScaled = 0;

    void Start()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        
    }

    public void ScaleUp()
    {
        if (timesScaled < maxTimesScaled)
        {
            timesScaled++;
            // Debug.Log( Mathf.Log(25 - timesScaled) - 2);
            // The more I grow the harder it becomes to grow
            transform.localScale = transform.localScale * (Mathf.Log(26 - timesScaled) - 2f);
        }
    }

    public void OnDestroy()
    {
        transform.localScale = initialScale;
    }
}
