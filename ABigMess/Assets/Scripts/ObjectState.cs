using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectState : MonoBehaviour
{

    public ObjectStates states;
    
    private void Start()
    {
        UpdateState();
    }


    private void Update()
    {

    }

    public void UpdateState()
    {
        if (states.washed && GetComponent<Washed>() == null)
        {
            gameObject.AddComponent<Washed>();
        }

        if (!states.washed && GetComponent<Washed>() != null)
        {
            Destroy(GetComponent<Washed>());
        }

        if (states.burnt && GetComponent<Burnt>() == null)
        {
            gameObject.AddComponent<Burnt>();
        }

        if (!states.burnt && GetComponent<Burnt>() != null)
        {
            Destroy(GetComponent<Burnt>());
        }

        if (states.grown && GetComponent<Grown>() == null)
        {
            gameObject.AddComponent<Grown>();
        }

        if (!states.grown && GetComponent<Grown>() != null)
        {
            Destroy(GetComponent<Grown>());
        }

        if (states.plugged && GetComponent<Plugged>() == null)
        {
            gameObject.AddComponent<Plugged>();
        }

        if (!states.plugged && GetComponent<Plugged>() != null)
        {
            Destroy(GetComponent<Plugged>());
        }
    }

    private void OnValidate()
    {
        UpdateState();
    }

    #region GET/SET

    public bool Washed
    {
        get
        {
            return states.washed;
        }

        set
        {
            states.washed = value;
            UpdateState();
        }
    }

    public bool Burnt
    {
        get
        {
            return states.burnt;
        }

        set
        {
            states.burnt = value;
            UpdateState();
        }
    }

    public bool Smuged
    {
        get
        {
            return states.smuged;
        }

        set
        {
            states.smuged = value;
            UpdateState();
        }
    }

    public bool Cooked
    {
        get
        {
            return states.cooked;
        }

        set
        {
            states.cooked = value;
            UpdateState();
        }
    }

    public bool Grown
    {
        get
        {
            return states.grown;
        }

        set
        {
            states.grown = value;
            UpdateState();
        }
    }

    public bool Colored
    {
        get
        {
            return states.colored;
        }

        set
        {
            states.colored = value;
            UpdateState();
        }
    }
    
    public bool Broken
    {
        get
        {
            return states.broken;
        }

        set
        {
            states.broken = value;
            UpdateState();
        }
    }
    
    public bool Opened
    {
        get
        {
            return states.opened;
        }

        set
        {
            states.opened = value;
            UpdateState();
        }
    }
    
    public bool Plugged
    {
        get
        {
            return states.plugged;
        }

        set
        {
            states.plugged = value;
            UpdateState();
        }
    }
    
    #endregion

    public enum State
    {
        washed,
        burnt,
        smuged,
        cooked,
        grown,
        colored,
        broken,
        opened,
        plugged
    }
}
