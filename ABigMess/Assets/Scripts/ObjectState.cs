using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class ObjectState : MonoBehaviour
{

    public ObjectStates states;

    private void Awake()
    {

    }

    private void Start()
    {
        UpdateState();
    }


    private void Update()
    {
        UpdateState();
    }

    private void InitStates()
    {
        states = new ObjectStates();
    }

    public void UpdateState()
    {
        if(states==null)
        {
            return;
        }
        if (states.washed && GetComponent<Washed>() == null)
        {
            gameObject.AddComponent<Washed>();
        }

        if (!states.washed && GetComponent<Washed>() != null)
        {
            DestroyImmediate(GetComponent<Washed>());
        }

        if (states.burnt && GetComponent<Burnt>() == null)
        {
            gameObject.AddComponent<Burnt>();
        }

        if (!states.burnt && GetComponent<Burnt>() != null)
        {
            DestroyImmediate(GetComponent<Burnt>());
        }

        if (states.cooked && GetComponent<Cooked>() == null)
        {
            gameObject.AddComponent<Cooked>();
        }

        if (!states.dirty && GetComponent<Dirty>() != null)
        {
            DestroyImmediate(GetComponent<Dirty>());
        }

        if (states.dirty && GetComponent<Dirty>() == null)
        {
            gameObject.AddComponent<Dirty>();
        }

        if (!states.cooked && GetComponent<Cooked>() != null)
        {
            DestroyImmediate(GetComponent<Cooked>());
        }

        if (states.grown && GetComponent<Grown>() == null)
        {
            gameObject.AddComponent<Grown>();
        }

        if (!states.grown && GetComponent<Grown>() != null)
        {
            DestroyImmediate(GetComponent<Grown>());
        }

        if (states.plugged && GetComponent<Plugged>() == null)
        {
            gameObject.AddComponent<Plugged>();
        }

        if (!states.plugged && GetComponent<Plugged>() != null)
        {
            DestroyImmediate(GetComponent<Plugged>());
        }
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

    public bool Dirty
    {
        get
        {
            return states.dirty;
        }

        set
        {
            states.dirty = value;
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
        plugged,
        dirty,
        none
    }
}
