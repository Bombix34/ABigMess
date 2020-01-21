using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlugCollider : MonoBehaviour
{

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GrabObject") && other.GetComponent<ObjectState>() != null)
        {
            print(this.transform.parent.gameObject + " collided with " + other.name);
            other.gameObject.GetComponent<ObjectState>().Plugged = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("GrabObject") && other.GetComponent<ObjectState>() != null)
        {
            other.gameObject.GetComponent<ObjectState>().Plugged = false;

        }
    }
}
