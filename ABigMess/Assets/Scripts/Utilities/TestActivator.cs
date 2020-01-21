using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestActivator : MonoBehaviour
{
    public GameObject selectedObject;
    public ToolSettings noToolInHand;

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

            if (hit.collider != null && hit.collider.gameObject != null)
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
                selectedObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            //Vector3 objPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
            //objPos.y = 0.5f;
            
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            Physics.Raycast(ray, out hit);

            if(hit.collider != null && hit.collider.gameObject != null)
            {
                GameObject gameObject = hit.collider.gameObject;
                noToolInHand.ApplyEvent(gameObject.GetComponent<InteractObject>());
            }
        }
    }



    public void OnCollide(GameObject gameObject)
    {
        if (selectedObject != null)
        {
            print(selectedObject.name + " collided with " + gameObject.name);
            if (gameObject.GetComponent<CollideEvent>().collision != null)
            {
                selectedObject.GetComponent<InteractObject>().Interact(gameObject.GetComponent<CollideEvent>().collision.gameObject);
            }
            if (selectedObject.GetComponent<InteractObject>() != null)
            {
                selectedObject.GetComponent<InteractObject>().Interact(gameObject);
            } 

        }
    }

}
