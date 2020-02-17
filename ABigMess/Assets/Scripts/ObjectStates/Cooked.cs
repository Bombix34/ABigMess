using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooked : MonoBehaviour
{
    Material defaultMaterial;
    ObjectState objectState;

    ParticleSystem cookedParticles;

    void Start()
    {
        objectState = GetComponent<ObjectState>();

        defaultMaterial = GetComponent<Renderer>().material;

        InteractObject interactObject = GetComponent<InteractObject>();

        if (interactObject != null && interactObject.Settings != null && interactObject.Settings.cookedMaterial != null)
        {
            GetComponent<Renderer>().material = interactObject.Settings.cookedMaterial;
        }
    }

    private void OnDestroy()
    {
        GetComponent<Renderer>().material = defaultMaterial;
        Destroy(cookedParticles);
    }


    public void CookedParticles(ParticleSystem cookedFixedParticles)
    {
        if (cookedFixedParticles != null)
        {
            cookedParticles = Instantiate(cookedFixedParticles, transform.position, Quaternion.identity);
        }
    }
}
