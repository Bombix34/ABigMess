using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;


public class PlayerRenderer : MonoBehaviour
{
    private PlayerManager manager;

    [SerializeField]
    private Color highlightToolsColor;
    [SerializeField]
    private Color highlightObjectsColor;

    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Animator quackAnimator;

    [SerializeField]
    private RopeBuilder leftArm, rightArm;

    [SerializeField]
    private Transform leftHandPositionForBring, rightHandPositionForBring;

    [SerializeField]
    private LayerMask layerMask;

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

    public void QuackAnim()
    {
        quackAnimator.SetTrigger("Quack");
    }

    /// <summary>
    /// return true if the hand is attached
    /// </summary>
    /// <param name="handBasePosition"></param>
    /// <param name="currentHand"></param>
    /// <returns></returns>
    private bool AttachOneHand(Vector3 handBasePosition, RopeBuilder currentHand)
    {
        Vector3 dirVector = (manager.Movement.GetFrontPosition() - handBasePosition);
        float distance = dirVector.magnitude;
        RaycastHit[] hits = Physics.RaycastAll(handBasePosition,dirVector.normalized,distance*10f);
        for(int i = 0; i < hits.Length; ++i)
        {
            if(hits[i].transform.gameObject == manager.GrabbedObject)
            {
                currentHand.HandPosition.position = hits[i].point;
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

    public Color HighlightToolsColor
    {
        get => highlightToolsColor;
    }

    public Color HighlightObjectsColor
    {
        get => highlightObjectsColor;
    }
}
