using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            if((hit.collider.gameObject==this.gameObject || hit.collider.gameObject==this.transform.parent.gameObject))
            {
                isActive = false;
                StartCoroutine(SphereAnim(false));
            }
            else if(!isActive)
            {
                isActive = true;
                StartCoroutine(SphereAnim(true));
            }
        }
    }

    private IEnumerator SphereAnim(bool isShowing)
    {
        if(isShowing)
        {
            while(this.transform.localScale.x<maskSize.x)
            {
                this.transform.localScale = new Vector3(this.transform.localScale.x + (Time.deltaTime*speedAnim), this.transform.localScale.x + (Time.deltaTime * speedAnim), this.transform.localScale.x + (Time.deltaTime * speedAnim));
                yield return new WaitForSeconds(0.001f);
            }
        }
        else
        {
            while (this.transform.localScale.x > 0f)
            {
                this.transform.localScale = new Vector3(this.transform.localScale.x - (Time.deltaTime * speedAnim), this.transform.localScale.x - (Time.deltaTime * speedAnim), this.transform.localScale.x - Time.deltaTime);
                yield return new WaitForSeconds(0.001f);
            }
        }
    }

    
}
