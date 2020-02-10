using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SeeThroughWalls : MonoBehaviour
{
    public LayerMask mask;

    public Vector3 maskSize;

    private bool isActive = false;

    public float speedAnim;
    
    private void Update()
    {
        Vector3 camPos = Camera.main.transform.position;
        RaycastHit hit;
        if(Physics.Raycast(camPos, (this.transform.position- camPos).normalized,out hit, Mathf.Infinity, mask))
        {
            if((hit.collider.gameObject==this.gameObject || hit.collider.gameObject.layer==this.gameObject.layer || !hit.collider.gameObject.CompareTag("Wall")))
            {
                isActive = false;
                this.transform.DOScale(0f, speedAnim/2);
            }
            else if(!isActive)
            {
                isActive = true;
                this.transform.DOScale(maskSize, speedAnim);
            }
        }
        else if(isActive)
        {
            isActive = false;
            this.transform.DOScale(0f, speedAnim / 2);
        }
    }

}

    

