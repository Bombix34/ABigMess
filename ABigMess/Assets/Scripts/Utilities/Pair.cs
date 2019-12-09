using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Pair<TKey, TValue> 
{
    public Pair(TKey key, TValue value)
    {
        Key = key;
        Value = value;
    }

    public Pair()
    {
    }

    public TKey Key;
    public TValue Value;
}

[Serializable]
public class BoolPair
{
    public bool Key;
    public bool Value;

    public BoolPair(bool key, bool value)
    {
        Key = key;
        Value = value;
    }
}
