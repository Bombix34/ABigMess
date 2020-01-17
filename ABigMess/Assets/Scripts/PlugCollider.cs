using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlugCollider : MonoBehaviour
{

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("GrabObject")&&other.GetComponent<InteractObject>()!=null)
        {

        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("GrabObject") && other.GetComponent<InteractObject>() != null)
        {

        }
    }
}
