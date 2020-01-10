using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[Serializable]
public class ObjectStates
{
    public BoolPair washed;
    public BoolPair burnt;
    public BoolPair smuged;
    public BoolPair cooked;
    public BoolPair grown;
    public BoolPair colored;
    public BoolPair broken;
    public BoolPair opened;
    public BoolPair plugged;

    public bool CompareBoolPair(BoolPair pair, bool value)
    {
        return pair.Key && pair.Value != value;
    }

    public override bool Equals(object obj)
    {
        if (obj is ObjectStates)
        {
            ObjectStates verif = (ObjectStates)obj;

            if(CompareBoolPair(verif.washed, washed.Value))
            {
                return false;
            }
            if (CompareBoolPair(verif.burnt, burnt.Value))
            {
                return false;
            }
            if (CompareBoolPair(verif.smuged, smuged.Value))
            {
                return false;
            }
            if (CompareBoolPair(verif.cooked, cooked.Value))
            {
                return false;
            }
            if (CompareBoolPair(verif.grown, grown.Value))
            {
                return false;
            }
            if (CompareBoolPair(verif.colored, colored.Value))
            {
                return false;
            }
            if (CompareBoolPair(verif.broken, broken.Value))
            {
                return false;
            }
            if (CompareBoolPair(verif.opened, opened.Value))
            {
                return false;
            }
            if (CompareBoolPair(verif.plugged, plugged.Value))
            {
                return false;
            }
            return true;
        }
        else
        {
            return false;
        }
    }
}

