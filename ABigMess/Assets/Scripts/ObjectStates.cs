using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[Serializable]
public class ObjectStates
{
    public ObjectStates()
    {
        washed = false;
        burnt = false;
        smuged = false;
        cooked = false;
        grown = false;
        colored = false;
        broken = false;
        opened = false;
        plugged = false;
    }

    public bool washed;
    public bool burnt;
    public bool smuged;
    public bool cooked;
    public bool grown;
    public bool colored;
    public bool broken;
    public bool opened;
    public bool plugged;
}

