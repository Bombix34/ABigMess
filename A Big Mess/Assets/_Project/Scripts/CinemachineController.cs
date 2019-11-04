using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineController : MonoBehaviour
{

    float speed = 10f;
    float rotationSpeed = 10f;

    public Transform cubePosition;
    private GameObject holdCube;
    private Transform saveCubeTransformParent;

    public Animator animator;
    private Rigidbody rigidbody;

    public RaycastSettings raycastSettings = new RaycastSettings();

    public BoxCollider holdCubeBoxCollider;
    public BoxCollider saveBoxCollider;

    [System.Serializable]
    public class RaycastSettings
    {
        public float ViewRange = 10;
        public float maxDistance = 2;
        public float hHalfFov = 45; // Horizontal half-field of view
        public float vHalfFov = 45; // Vertical half-field of view
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    enum Direction
    {
        TOP,
        BOTTOM,
        LEFT,
        RIGHT
    }

    // Update is called once per frame
    void Update()
    {
        float translation = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float rotation = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

        rigidbody.MovePosition(new Vector3(rotation + transform.position.x, transform.position.y, transform.position.z + translation));


        if (translation > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (translation < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        if (rotation > 0)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else if (rotation < 0)
        {
            transform.rotation = Quaternion.Euler(0, 270, 0);
        }


        if (Input.GetButtonDown("Jump"))
        {
            animator.SetTrigger("isJumping");
        }

        if (translation != 0 || rotation != 0)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }


        FindNearestObjectToPickUp();
        

        if (holdCube != null)
        {
            holdCube.transform.position = Vector3.Lerp(holdCube.transform.position, cubePosition.position, Time.deltaTime * speed);
            holdCube.transform.rotation = Quaternion.Slerp(holdCube.transform.rotation, cubePosition.rotation, Time.deltaTime * speed);
        }

        if (holdCube != null && Input.GetMouseButtonUp(0))
        {
            DropHoldCube();
        }
    }

    public void DropHoldCube()
    {
        RemoveHoldCubeCollider();
        holdCube.transform.parent = saveCubeTransformParent;
        holdCube.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        holdCube = null;
        
    }

    void FindNearestObjectToPickUp()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, raycastSettings.ViewRange);

        int layerMask = 8;

        float minDistance = float.MaxValue;
        Collider minHit = null;

        foreach (Collider hit in hits) // Field of view of raycast
        {
            if (hit.gameObject == gameObject) continue;

            Vector3 direction = (transform.position - hit.transform.position);
            Vector3 hDirn = Vector3.ProjectOnPlane(direction, transform.up).normalized;
            Vector3 vDirn = Vector3.ProjectOnPlane(direction, transform.forward).normalized; // forward ?

            float hOffset = Vector3.Dot(hDirn, transform.forward) * Mathf.Rad2Deg;
            float vOffset = Vector3.Dot(vDirn, transform.forward) * Mathf.Rad2Deg;

            if (hOffset > raycastSettings.hHalfFov || vOffset > raycastSettings.vHalfFov)
            {
                continue;
            }

            float distance = Vector3.Distance(transform.position, hit.transform.position);

            if (distance < minDistance && distance < raycastSettings.maxDistance && hit.transform.gameObject.layer != layerMask)
            {
                minDistance = distance;
                minHit = hit;
            }
        }

        if (minHit != null)
        {
            Debug.DrawLine(transform.position, minHit.transform.position, Color.red);
        }

        if(minHit != null && Input.GetButtonDown("Fire1"))
        {
            saveCubeTransformParent = minHit.gameObject.transform.parent;
            holdCube = minHit.gameObject;
            minHit.transform.parent = cubePosition.transform;
            
            holdCube.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            AddHoldCubeCollider();
        }
    }

    // Adds a cube collider to the player in order for the cube not to go through walls
    void AddHoldCubeCollider()
    {
        holdCubeBoxCollider = gameObject.AddComponent<BoxCollider>();
        Debug.Log("Local Position: " + cubePosition.localPosition);
        holdCubeBoxCollider.center = cubePosition.localPosition;
        Destroy(holdCube.GetComponent<BoxCollider>());
    }

    void RemoveHoldCubeCollider()
    {
        Destroy(holdCubeBoxCollider);
        holdCube.AddComponent<BoxCollider>();
    }
}
