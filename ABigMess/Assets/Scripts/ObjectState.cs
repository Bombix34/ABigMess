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
        if (states.washed.Value && GetComponent<Washed>() == null)
        {
            gameObject.AddComponent<Washed>();
        }

        if (!states.washed.Value && GetComponent<Washed>() != null)
        {
            Destroy(GetComponent<Washed>());
        }

        if (states.burnt.Value && GetComponent<Burnt>() == null)
        {
            gameObject.AddComponent<Burnt>();
        }

        if (!states.burnt.Value && GetComponent<Burnt>() != null)
        {
            Destroy(GetComponent<Burnt>());
        }

        if (states.grown.Value && GetComponent<Grown>() == null)
        {
            gameObject.AddComponent<Grown>();
        }

        if (!states.grown.Value && GetComponent<Grown>() != null)
        {
            Destroy(GetComponent<Grown>());
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
            return states.washed.Value;
        }

        set
        {
            states.washed.Value = value;
            UpdateState();
        }
    }

    public bool Burnt
    {
        get
        {
            return states.burnt.Value;
        }

        set
        {
            states.burnt.Value = value;
            UpdateState();
        }
    }

    public bool Smuged
    {
        get
        {
            return states.smuged.Value;
        }

        set
        {
            states.smuged.Value = value;
            UpdateState();
        }
    }

    public bool Cooked
    {
        get
        {
            return states.cooked.Value;
        }

        set
        {
            states.cooked.Value = value;
            UpdateState();
        }
    }

    public bool Grown
    {
        get
        {
            return states.grown.Value;
        }

        set
        {
            states.grown.Value = value;
            UpdateState();
        }
    }

    public bool Colored
    {
        get
        {
            return states.colored.Value;
        }

        set
        {
            states.colored.Value = value;
            UpdateState();
        }
    }
    
    public bool Broken
    {
        get
        {
            return states.broken.Value;
        }

        set
        {
            states.broken.Value = value;
            UpdateState();
        }
    }
    
    public bool Opened
    {
        get
        {
            return states.opened.Value;
        }

        set
        {
            states.opened.Value = value;
            UpdateState();
        }
    }
    
    public bool Plugged
    {
        get
        {
            return states.plugged.Value;
        }

        set
        {
            states.plugged.Value = value;
            UpdateState();
        }
    }
    
    #endregion
}
