using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "BIGMESS_EVENTS/Change Color Event")]
public class ChangeColorEvent : InteractEvent
{
    [SerializeField] List<Color> colors;

    public override void InteractionEvent(GameObject objConcerned)
    {
        SetupObjectState(objConcerned);
        TryInstantiateParticleFX(objConcerned);

        if (colors != null && colors.Count > 1)
        {
            Color choosenColor = colors[(int)Random.Range(0f, colors.Count)];
            while(choosenColor == objConcerned.GetComponent<Renderer>().material.color)
            {
                choosenColor = colors[(int)Random.Range(0f, colors.Count)];
            }
            if (MusicManager.Instance != null)
            {
                MusicManager.Instance.GetSoundManager().PaintSFX();
            }
            objConcerned.GetComponent<Renderer>().material.color = choosenColor;
        }
    }
}