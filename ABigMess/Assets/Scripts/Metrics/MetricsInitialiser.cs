using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetricsInitialiser : MonoBehaviour
{
    [SerializeField]
    private MetricsManager metricsDatas;

    private void Start()
    {
        if(metricsDatas.saveMetrics)
        {
            metricsDatas.Reset();
            metricsDatas.InitCSVText();
        }
    }

}
