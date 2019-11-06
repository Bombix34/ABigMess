using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInputManager))]

public class PlayerManager : ObjectManager
{
    public PlayerReglages reglages;

    PlayerInputManager inputs;

    CharacterController character;

    Vector3 currentVelocity;

    Transform mainCamera;

    Animator animator;

    GameObject interactObject= null;  //raycasted object in front of player
    GameObject grabbedObject=null;   //object currently hold/grabb

    [SerializeField]
    GameObject bringPosition;
   

    void Awake()
    {
        inputs = GetComponent<PlayerInputManager>();
        mainCamera = Camera.main.transform;
        character = GetComponent<CharacterController>();

        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
       // ChangeState(new PlayerBaseState(this));
    }

    private void Update()
    {
        RaycastObject();
        UpdateGrabbedObject();
        if(inputs.GetInteractInputDown())
        {
            print("interact");
        }
        if(inputs.GetGrabInputDown())
        {
            if (grabbedObject == null)
            {
                if (interactObject != null)
                {
                    grabbedObject = interactObject;
                    grabbedObject.GetComponent<Rigidbody>().useGravity = false;
                    interactObject = null;
                    print(grabbedObject);
                }
            }
            else
            {
                DropBringObject();
            }
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
            DoMove();
    }

    public override void ChangeState(State newState)
    {
        base.ChangeState(newState);
    }

    private void OnDrawGizmos()
    {

    }

    //MOVEMENT FUNCTIONS______________________________________________________________________________

    bool canMove = true;

    public void Move(bool IsOn)
    {
        canMove = IsOn;
    }

    public void DoMove()
    {
        Vector3 directionController = inputs.GetMovementInput();
        GravitySpeed();
        if (directionController == Vector3.zero)
        {
            currentVelocity = Vector3.zero;
            UpdateAnim();
            return;
        }
        //init values
        Vector3 forward = mainCamera.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        Vector3 right = mainCamera.transform.right;

        Vector3 rightMove = right * (10 * inputs.GetMovementInputX()) * Time.deltaTime;
        Vector3 upMove = forward * (10 * inputs.GetMovementInputY()) * Time.deltaTime;
        Vector3 heading = (rightMove + upMove).normalized;

        float amplitude = new Vector2(inputs.GetMovementInputX(), inputs.GetMovementInputY()).magnitude;

        RotatePlayer(inputs.GetMovementInputY(), -inputs.GetMovementInputX());
        currentVelocity = Vector3.zero;
        currentVelocity += heading * amplitude * (reglages.moveSpeed / 5f);
        character.Move(currentVelocity);
        UpdateAnim();
    }

    public void GravitySpeed()
    {
        if (!character.isGrounded)
        {
            currentVelocity = Vector3.zero;
            float gravity = reglages.gravity * Time.deltaTime;
            currentVelocity = new Vector3(currentVelocity.x, currentVelocity.y - gravity, currentVelocity.z);
            character.Move(currentVelocity);
        }
    }

    public void ResetVelocity()
    {
        character.Move(Vector3.zero);
        animator.SetFloat("MoveSpeed", 0f);
    }

    private void RotatePlayer(float x, float y)
    {
        Vector3 dir = new Vector3(-y, 0, x);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dir), (reglages.rotationSpeed * 100) * Time.deltaTime);
    }

    private void UpdateAnim()
    {
        if (animator == null)
            return;
       // animator.SetFloat("MoveSpeed", currentVelocity.magnitude / 0.1f);
       animator.SetBool("isRunning", currentVelocity.magnitude > 0); 
    }

    void UpdateGrabbedObject()
    {
        if (grabbedObject == null)
            return;
        grabbedObject.transform.position = Vector3.Lerp(grabbedObject.transform.position, bringPosition.transform.position, Time.deltaTime * 10f);
        grabbedObject.transform.rotation = Quaternion.Slerp(grabbedObject.transform.rotation, bringPosition.transform.rotation, Time.deltaTime * 10f);
    }

    public void DropBringObject()
    {
        grabbedObject.transform.parent = null;
        grabbedObject.GetComponent<Rigidbody>().useGravity = true;
        grabbedObject = null;
    }

    //RAYCAST OBJECTS___________________________________________________________________________________

    public void RaycastObject()
    {
        bool isResult = false;
        GameObject raycastObject = null;
        Vector3 testPosition = GetFrontPosition();

        Collider[] hitColliders = Physics.OverlapSphere(testPosition, reglages.raycastRadius);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if(hitColliders[i].gameObject.tag=="GrabObject")
            {
                interactObject = hitColliders[i].gameObject;
                isResult = true;
                break;
            }
            i++;
        }
        if (!isResult)
        {
           // if(interactObject!=null)
            //    interactObject.GetComponent<InteractObject>().UpdateFeedback(false);
            interactObject = null;
        }
    }

    /*
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
    */

    public Vector3 GetFrontPosition()
    {
        //FONCTION POUR OBTENIR LA POSITION DEVANT LE PERSONNAGE
        //POSITION OU INTERAGIR ET POSER LES OBJETS
        Vector3 forwardPos = transform.TransformDirection(Vector3.forward) * 0.5f;
        Vector3 testPosition = new Vector3(transform.position.x + forwardPos.x,
            transform.position.y + forwardPos.y,
            transform.position.z + forwardPos.z);
        return testPosition;
    }

    public Vector3 GetHeadingDirection()
    {
        return transform.TransformDirection(Vector3.forward);
    }

    //GET & SET________________________________________________________________________________________

    public PlayerInputManager GetInputManager()
    {
        return inputs;
    }
    
}
