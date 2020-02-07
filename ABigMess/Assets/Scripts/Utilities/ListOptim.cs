using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ListOptim<T>
{
    public T[] Values;
    public int Count = 0;

    public ListOptim(int size)
    {
        Values = new T[size];
    }

    public void Add(T value)
    {
        Values[Count++] = value;
    }

    public void RemoveAt(int i)
    {
        if (i < Count - 1)
        {
            Values[i] = Values[Count - 1];
        }
        Count--;
    }

    public T this[int i]
    {
        get => Values[i];
        set => Values[i] = value;
    }
    
    public void Clear()
    {
        Count = 0;
    }
}
