using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractEvent : ScriptableObject
{
    /// <summary>
    /// Script for interaction object that need to be set when the game launch
    /// exemple : the radio, maybe some other stuff
    /// </summary>
    public abstract void InteractionEvent(GameObject objConcerned);
}
