using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestActivator : MonoBehaviour
{
    public GameObject selectedObject;
    public GameObject actionObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            Physics.Raycast(ray, out hit);

            if (hit.collider.gameObject != null && selectedObject == null)
            {
                selectedObject = hit.collider.gameObject;
            } 
            else if (hit.collider.gameObject != null && selectedObject != null)
            {
                actionObject = hit.collider.gameObject;
            }

            ApplyEvents();
        }

        if(selectedObject != null)
        {
            //Vector3 objPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
            //objPos.y = 0.5f;
            //selectedObject.transform.position = objPos;
        }

        
    }


    public void ApplyEvents()
    {
        if(actionObject != null && selectedObject != null)
        {
            selectedObject.GetComponent<InteractObject>().Interact(actionObject);


            actionObject = null;
            selectedObject = null;
        }
    }

}
