using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class InteractEvent : ScriptableObject
{
    [SerializeField] private BoolPair washed;
    [SerializeField] private BoolPair dirty;
    [SerializeField] private BoolPair burnt;
    [SerializeField] private BoolPair smuged;
    [SerializeField] private BoolPair cooked;
    [SerializeField] private BoolPair grown;
    [SerializeField] private BoolPair colored;
    [SerializeField] private BoolPair broken;
    [SerializeField] private BoolPair opened;
    [SerializeField] private BoolPair plugged;

    [SerializeField]
    GameObject particleFX;

    [SerializeField]
    float chronoBeforeKillingParticles = 0.25f;

    [SerializeField]
    public Sprite icon;


    /// <summary>
    /// Script for interaction object that need to be set when the game launch
    /// exemple : the radio, maybe some other stuff
    /// </summary>
    public abstract void InteractionEvent(GameObject objConcerned);

    protected GameObject TryInstantiateParticleFX(GameObject objConcerned)
    {
        if (particleFX != null)
        {
            GameObject particles = Instantiate(particleFX, objConcerned.transform.position, Quaternion.identity);
            Destroy(particles.gameObject, chronoBeforeKillingParticles);
            return particles.gameObject;
        }
        return null;
    }

    public void SetupObjectState(GameObject objConcerned)
    {
        if(objConcerned.GetComponent<ObjectState>() == null)
        {
            objConcerned.AddComponent<ObjectState>();
        }

        SetStates(objConcerned);

        objConcerned.GetComponent<ObjectState>().UpdateState();
    }

    private void SetStates(GameObject objConcerned)
    {
        if (washed.Key)
        {
            objConcerned.GetComponent<ObjectState>().Washed = Washed;
        }
        if (dirty.Key)
        {
            objConcerned.GetComponent<ObjectState>().Dirty = Dirty;
        }
        if (burnt.Key)
        {
            objConcerned.GetComponent<ObjectState>().Burnt = Burnt;
        }
        if (smuged.Key)
        {
            objConcerned.GetComponent<ObjectState>().Smuged = Smuged;
        }
        if (cooked.Key)
        {
            objConcerned.GetComponent<ObjectState>().Cooked = Cooked;
        }
        if (grown.Key)
        {
            objConcerned.GetComponent<ObjectState>().Grown = Grown;
        }
        if (colored.Key)
        {
            objConcerned.GetComponent<ObjectState>().Colored = Colored;
        }
        if (broken.Key)
        {
            objConcerned.GetComponent<ObjectState>().Broken = Broken;
        }
        if (opened.Key)
        {
            objConcerned.GetComponent<ObjectState>().Opened = Opened;
        }
        if (plugged.Key)
        {
            objConcerned.GetComponent<ObjectState>().Plugged = Plugged;
        }
    }

    #region GET/SET

    public bool Washed
    {
        get
        {
            return washed.Value;
        }

        set
        {
            washed.Value = value;
        }
    }

    public bool Dirty
    {
        get
        {
            return dirty.Value;
        }

        set
        {
            dirty.Value = value;
        }
    }

    public bool Burnt
    {
        get
        {
            return burnt.Value;
        }

        set
        {
            burnt.Value = value;
        }
    }

    public bool Smuged
    {
        get
        {
            return smuged.Value;
        }

        set
        {
            smuged.Value = value;
        }
    }

    public bool Cooked
    {
        get
        {
            return cooked.Value;
        }

        set
        {
            cooked.Value = value;
        }
    }

    public bool Grown
    {
        get
        {
            return grown.Value;
        }

        set
        {
            grown.Value = value;
        }
    }

    public bool Colored
    {
        get
        {
            return colored.Value;
        }

        set
        {
            colored.Value = value;
        }
    }

    public bool Broken
    {
        get
        {
            return broken.Value;
        }

        set
        {
            broken.Value = value;
        }
    }

    public bool Opened
    {
        get
        {
            return opened.Value;
        }

        set
        {
            opened.Value = value;
        }
    }

    public bool Plugged
    {
        get
        {
            return plugged.Value;
        }

        set
        {
            plugged.Value = value;
        }
    }

    #endregion
}

