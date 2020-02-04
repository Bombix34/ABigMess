using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycast : MonoBehaviour
{
    [SerializeField]
    List<InteractObject> raycastedObjects;

    List<GameObject> raycastTemporary;

    PlayerManager manager;

    void Start()
    {
        raycastedObjects = new List<InteractObject>();
        raycastTemporary = new List<GameObject>();
        manager = this.GetComponentInParent<PlayerManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<InteractObject>()!=null || other.CompareTag("Wall"))
        {
            raycastTemporary.Add(other.gameObject);
            FilterRaycastedList();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<InteractObject>()!=null && !raycastedObjects.Contains(other.GetComponent<InteractObject>()))
        {
            raycastedObjects.Add(other.GetComponent<InteractObject>());
            FilterRaycastedList();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(raycastTemporary.Contains(other.gameObject))
        {
            raycastTemporary.Remove(other.gameObject);
        }
        if (other.GetComponent<InteractObject>() != null && raycastedObjects.Contains(other.GetComponent<InteractObject>()))
        {
            raycastedObjects.Remove(other.GetComponent<InteractObject>());
            FilterRaycastedList();
        }
    }

    private void FilterRaycastedList()
    {
        List<GameObject> walls = new List<GameObject>();
        raycastedObjects.Clear();
        for(int i =0; i < raycastTemporary.Count;++i)
        {
            if(raycastTemporary[i].CompareTag("Wall"))
            {
                walls.Add(raycastTemporary[i]);
            }
        }
        if (walls.Count == 0)
        {
            for (int j = 0; j < raycastTemporary.Count; ++j)
            {
                if (raycastTemporary[j] != manager.GrabbedObject)
                {
                    raycastedObjects.Add(raycastTemporary[j].GetComponent<InteractObject>());
                }
            }
        }
        else
        {
            float[] distanceFromPlayer = new float[walls.Count];
            for(int i = 0; i < walls.Count;++i)
            {
                distanceFromPlayer[i] = (walls[i].transform.position-this.transform.parent.transform.position).magnitude;
            }
            for (int j = 0; j < raycastTemporary.Count; ++j)
            {
                if (raycastTemporary[j].GetComponent<InteractObject>() != null)
                {
                    float distance = (raycastTemporary[j].transform.position - this.transform.position).magnitude;
                    bool canAdd = true;
                    for(int z = 0; z < distanceFromPlayer.Length; ++z)
                    {
                        if(distanceFromPlayer[z]<distance)
                        {
                            canAdd = false;
                        }
                    }
                    if(canAdd)
                    {
                        if(raycastTemporary[j]!=manager.GrabbedObject)
                        {
                            raycastedObjects.Add(raycastTemporary[j].GetComponent<InteractObject>());
                        }
                    }
                }
            }
        }
    }

    public void RemoveFromRaycast(InteractObject other)
    {
        if (raycastedObjects.Contains(other))
        {
            raycastedObjects.Remove(other);
            FilterRaycastedList();
        }
    }

    public List<InteractObject> GetRaycastedObjects()
    {
        return raycastedObjects;
    }

    public int GetRaycastedObjectsCount()
    {
        return raycastedObjects.Count;
    }

    public InteractObject GetUpperRaycastedObject()
    {
        if(raycastedObjects.Count==0)
        {
            return null;
        }
        InteractObject result = raycastedObjects[0];
        for (int i = 1; i < raycastedObjects.Count; ++i)
        {
            if (raycastedObjects[i].transform.position.y > result.transform.position.y)
            {
                result = raycastedObjects[i];
            }
        }
        return result;
    }
}
