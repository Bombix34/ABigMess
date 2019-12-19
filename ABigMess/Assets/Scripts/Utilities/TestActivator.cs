using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestActivator : MonoBehaviour
{
    public GameObject selectedObject;

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

            if (hit.collider.gameObject != null)
            {
                selectedObject = hit.collider.gameObject;
            } 
        }

        if (Input.GetMouseButton(0) && selectedObject != null)
        {
            Plane plane = new Plane(Vector3.up, new Vector3(0, 0.5f, 0));
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float distance;

            if(plane.Raycast(ray, out distance))
            {
                selectedObject.transform.position = ray.origin + (ray.direction * distance);
                selectedObject.transform.rotation = Quaternion.identity;
            }
            //Vector3 objPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
            //objPos.y = 0.5f;
            
        }

        
    }



    public void OnCollide(GameObject gameObject)
    {
        if (selectedObject != null)
        {
            print(gameObject.name + " collided with " + selectedObject.name);
            selectedObject.GetComponent<InteractObject>().Interact(gameObject);
        }
    }

}
