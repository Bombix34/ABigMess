using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTopDownController : MonoBehaviour
{

    float speed = 10f;

    public Transform cubePostion;
    public GameObject holdCube;
    public Transform saveCubeTransformParent;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float translationX = Input.GetAxis("Vertical") * Time.deltaTime * speed;
        float translationY = Input.GetAxis("Horizontal") * Time.deltaTime * speed;

        transform.Translate(0, 0, translationX, Space.World);
        transform.Translate(translationY, 0, 0, Space.World);
     
        Vector3 localPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 mousePos = Input.mousePosition - localPos;

        transform.rotation = Quaternion.Euler(0, -(Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg) + 90.0f, 0);

        Debug.DrawRay(transform.position, transform.forward, Color.black);

        if (Input.GetButtonDown("Jump"))
        {
            animator.SetTrigger("isJumping");
        }

        if(translationX != 0 || translationY != 0)
        {
            animator.SetBool("isRunning", true);
        } else
        {
            animator.SetBool("isRunning", false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            int layerMask = 1 << 8;
            layerMask = ~layerMask;

            if (Physics.Raycast(transform.position, transform.forward, out hit, 2f, layerMask))
            {
                saveCubeTransformParent = hit.collider.gameObject.transform.parent;
                hit.collider.gameObject.transform.parent = cubePostion;
                holdCube = hit.collider.gameObject;
            }

        }

        if (holdCube != null)
        {
            holdCube.transform.position = Vector3.Lerp(holdCube.transform.position, cubePostion.position, Time.deltaTime * speed);
            holdCube.transform.rotation = Quaternion.Slerp(holdCube.transform.rotation, cubePostion.rotation, Time.deltaTime * speed);
        }

        if (holdCube != null && Input.GetMouseButtonUp(0))
        {
            holdCube.transform.parent = saveCubeTransformParent;
            holdCube = null;
        }
    }


}
