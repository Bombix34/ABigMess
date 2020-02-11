using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Billboard : MonoBehaviour
{
    void LateUpdate()
    {
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
            Camera.main.transform.rotation * Vector3.up);
        Vector3 eulerRot = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, eulerRot.y,-15f);
    }
}
