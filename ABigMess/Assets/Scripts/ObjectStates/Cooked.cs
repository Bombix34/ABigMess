using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooked : MonoBehaviour
{
    Color defaultColor;
    ObjectState objectState;

    ParticleSystem cookedParticles;
    
    void Start()
    {
        defaultColor = GetComponent<Renderer>().material.color;
        objectState = GetComponent<ObjectState>();

        GetComponent<Renderer>().material.color = Color.yellow;
    }

    private void OnDestroy()
    {
        GetComponent<Renderer>().material.color = defaultColor;
        Destroy(cookedParticles);
    }

    internal void CookedParticles(ParticleSystem cookedFixedParticles)
    {
        if(cookedFixedParticles != null)
        {
            cookedParticles = Instantiate(cookedFixedParticles, transform.position, Quaternion.identity);
        }
    }
}
