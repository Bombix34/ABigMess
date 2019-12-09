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
        }
    }
    
    #endregion
}
