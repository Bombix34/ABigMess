using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointBreak : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnJointBreak(float breakForce)
    {
        transform.parent.GetComponent<PlayerManager>().JointBroken(this);
    }
}
