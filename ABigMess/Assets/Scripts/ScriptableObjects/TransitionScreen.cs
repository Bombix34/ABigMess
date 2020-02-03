using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "BIGMESS_LEVELS/new transition screen")]
public class TransitionScreen : ScriptableObject 
{
    public Color backgroundColor;
    [TextArea(2,5)]
    public string textDescription;
}
