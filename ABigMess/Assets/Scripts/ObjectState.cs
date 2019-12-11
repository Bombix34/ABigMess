using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectState : MonoBehaviour
{

    [SerializeField] private bool washed;
    [SerializeField] private bool burnt;
    [SerializeField] private bool smuged;
    [SerializeField] private bool cooked;
    [SerializeField] private bool grown;
    [SerializeField] private bool colored;
    [SerializeField] private bool broken;
    [SerializeField] private bool opened;
    [SerializeField] private bool plugged;


    private void Start()
    {
        UpdateState();
    }


    private void Update()
    {

        

    }

    public void UpdateState()
    {
        if (washed && GetComponent<Washed>() == null)
        {
            gameObject.AddComponent<Washed>();
        }

        if (!washed && GetComponent<Washed>() != null)
        {
            Destroy(GetComponent<Washed>());
        }

        if (burnt && GetComponent<Burnt>() == null)
        {
            gameObject.AddComponent<Burnt>();
        }

        if (!burnt && GetComponent<Burnt>() != null)
        {
            Destroy(GetComponent<Burnt>());
        }
    }

    #region GET/SET

    public bool Washed
    {
        get
        {
            return washed;
        }

        set
        {
            washed = value;
            UpdateState();
        }
    }

    public bool Burnt
    {
        get
        {
            return burnt;
        }

        set
        {
            burnt = value;
            UpdateState();
        }
    }

    public bool Smuged
    {
        get
        {
            return smuged;
        }

        set
        {
            smuged = value;
            UpdateState();
        }
    }

    public bool Cooked
    {
        get
        {
            return cooked;
        }

        set
        {
            cooked = value;
            UpdateState();
        }
    }

    public bool Grown
    {
        get
        {
            return grown;
        }

        set
        {
            grown = value;
            UpdateState();
        }
    }

    public bool Colored
    {
        get
        {
            return colored;
        }

        set
        {
            colored = value;
            UpdateState();
        }
    }
    
    public bool Broken
    {
        get
        {
            return broken;
        }

        set
        {
            broken = value;
            UpdateState();
        }
    }
    
    public bool Opened
    {
        get
        {
            return opened;
        }

        set
        {
            opened = value;
            UpdateState();
        }
    }
    
    public bool Plugged
    {
        get
        {
            return plugged;
        }

        set
        {
            plugged = value;
            UpdateState();
        }
    }
    
    #endregion
}
