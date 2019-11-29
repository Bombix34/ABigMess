using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSystem : MonoBehaviour
{
    [SerializeField]
    GameObject partPrefab, parentObject;

    [SerializeField]
    [Range(1, 100)]
    int lenght = 1;

    [SerializeField]
    float partdistance = 0.21f;

    [SerializeField]
    bool reset, spawn, snapFirst, snapLast;

    private List<GameObject> curRope;


    private void Start()
    {
        curRope = new List<GameObject>();
    }

    void Update()
    {
        if(reset)
        {
            foreach(var item in curRope)
            {
                Destroy(item);
            }
            curRope.Clear();
            reset = false;
        }
        if(spawn)
        {
            Spawn();
            spawn = false;
        }

    }

    public void Spawn()
    {
        int count = (int)(lenght / partdistance);
        for(int index=0; index< count; ++index)
        {
            GameObject tmpPart = Instantiate(partPrefab, new Vector3(transform.position.x, transform.position.y +( partdistance * (index + 1)), transform.position.z), Quaternion.identity, parentObject.transform);
            curRope.Add(tmpPart);
            tmpPart.name = curRope.Count.ToString();
            if(index==0)
            {
                Destroy(tmpPart.GetComponent<CharacterJoint>());
                if(snapFirst)
                {
                    tmpPart.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                }
            }
            else
            {
                tmpPart.GetComponent<CharacterJoint>().connectedBody = curRope[index - 1].GetComponent<Rigidbody>();
            }
        }
        if(snapLast)
        {
            curRope[curRope.Count - 1].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}
