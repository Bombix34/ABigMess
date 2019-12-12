using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;


public class PlayerRenderer : MonoBehaviour
{
    PlayerManager manager;

    [SerializeField]
    RopeBuilder leftArm, rightArm;

    [SerializeField]
    Transform leftHandPositionForBring, rightHandPositionForBring;


    void Awake()
    {
        manager = GetComponent<PlayerManager>();
    }

    public void AttachHandToObject()
    {

        //leftArm.HandPosition.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        //rightArm.HandPosition.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        MoveArmsToBringPosition();

        RaycastHit hit;
        LayerMask layerMask = ~(1 << LayerMask.NameToLayer("Player"));
        Vector3 dirVector = (manager.GrabbedObject.transform.position - leftHandPositionForBring.position).normalized;
        if (Physics.Raycast(leftHandPositionForBring.position, dirVector, out hit, Mathf.Infinity, layerMask))
        {
            leftArm.HandPosition.position = hit.point;
        }
        Debug.DrawRay(leftArm.HandPosition.position, dirVector, Color.yellow);

        dirVector = (manager.GrabbedObject.transform.position - rightHandPositionForBring.position).normalized;
        if (Physics.Raycast(rightHandPositionForBring.position, dirVector, out hit, Mathf.Infinity, layerMask))
        {
            rightArm.HandPosition.position = hit.point;
        }

        leftArm.HandPosition.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        rightArm.HandPosition.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }

    /// <summary>
    /// WIP 
    /// Move the arms to a position in front of the player to help positioning correctly when bring object
    /// </summary>
    private void MoveArmsToBringPosition()
    {
        //StartCoroutine(MoveArmsCoroutine(leftArm.HandPosition,-0.6f));
        //StartCoroutine(MoveArmsCoroutine(rightArm.HandPosition, 0.6f));
        leftArm.HandPosition.position = leftHandPositionForBring.position;
        rightArm.HandPosition.position = rightHandPositionForBring.position;
    }

    public void DetachHand()
    {
        leftArm.HandPosition.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        rightArm.HandPosition.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }

    /*
    IEnumerator MoveArmsCoroutine(Transform handToMove, float posModif)
    {
        Vector3 leftArmFinalPos = new Vector3(armsPositionForBring.position.x + posModif, armsPositionForBring.position.y, armsPositionForBring.position.z);
        Vector3 dirVector = (leftArmFinalPos - handToMove.localPosition).normalized;
      //  Debug.DrawLine(handToMove.position, leftArmFinalPos);
        Debug.DrawRay(handToMove.localPosition, dirVector);
        Debug.Break();
        while (handToMove.localPosition != leftArmFinalPos)
        {
            handToMove.Translate(dirVector * Time.fixedDeltaTime);
            yield return new WaitForSeconds(0.001f);
        }
       // leftArm.HandPosition.Translate(new Vector3(armsPositionForBring.position.x - 0.6f, armsPositionForBring.position.y, armsPositionForBring.position.z);
    }
    */
}
