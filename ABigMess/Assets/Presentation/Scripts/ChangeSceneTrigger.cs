using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneTrigger : MonoBehaviour
{
    public Object sceneToChange;

    public PresentationTransition presentationTransition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            presentationTransition.LeaveSceneTransition();
        }
    }
}
