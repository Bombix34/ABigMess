using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;


public class PlayerRenderer : MonoBehaviour
{
    private PlayerManager manager;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private RopeBuilder leftArm, rightArm;

    [SerializeField]
    private Transform leftHandPositionForBring, rightHandPositionForBring;

    private bool leftHandNeedGrab = false;
    private bool rightHandNeedGrab = false;

    private void Awake()
    {
        manager = GetComponent<PlayerManager>();
    }

    private void Update()
    {
        UpdateHandPosition();
    }

    public void UpdateAnimation(float inputMagnitude)
    {
        animator.SetFloat("MovementSpeed", inputMagnitude);
    }

    private void UpdateHandPosition()
    {
        if(leftHandNeedGrab)
        {
            leftHandNeedGrab = !AttachOneHand(leftHandPositionForBring.position, leftArm);
        }
        if(rightHandNeedGrab)
        {
            rightHandNeedGrab = !AttachOneHand(rightHandPositionForBring.position, rightArm);
        }
    }

    public void AttachHandToObject()
    {
        MoveArmsToBringPosition();

        leftHandNeedGrab = true;
        rightHandNeedGrab = true;
    }

    /// <summary>
    /// return true if the hand is attached
    /// </summary>
    /// <param name="handBasePosition"></param>
    /// <param name="currentHand"></param>
    /// <returns></returns>
    private bool AttachOneHand(Vector3 handBasePosition, RopeBuilder currentHand)
    {
        RaycastHit hit;
        LayerMask layerMask = ~(0 << LayerMask.NameToLayer("Player"));
        Vector3 objectPosition = manager.GrabbedObject.transform.position;
        float distance = Mathf.Sqrt(Mathf.Pow(objectPosition.x-handBasePosition.x,2f)+ Mathf.Pow(objectPosition.y - handBasePosition.y, 2f)+ Mathf.Pow(objectPosition.z - handBasePosition.z, 2f));
        Vector3 dirVector = (objectPosition - handBasePosition).normalized;

        if (Physics.Raycast(handBasePosition, dirVector, out hit, distance*10f, layerMask))
        {
            if (hit.transform.gameObject == manager.GrabbedObject)
            {
                currentHand.HandPosition.position = hit.point;
                currentHand.HandPosition.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Move the arms to a position in front of the player to help positioning correctly when bring object
    /// </summary>
    private void MoveArmsToBringPosition()
    {
        leftArm.HandPosition.position = leftHandPositionForBring.position;
        rightArm.HandPosition.position = rightHandPositionForBring.position;
    }

    public void DetachHand()
    {
        leftHandNeedGrab = false;
        rightHandNeedGrab = false;

        leftArm.HandPosition.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        rightArm.HandPosition.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }

    public RopeBuilder RightArm
    {
        get => rightArm;
    }
}
