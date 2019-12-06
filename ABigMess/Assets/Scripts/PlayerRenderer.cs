using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;


public class PlayerRenderer : MonoBehaviour
{
    PlayerManager manager;

    [SerializeField]
    List<RopeBuilder> arms;

    [SerializeField]
    Transform armsPositionForBring;


    void Awake()
    {
        manager = GetComponent<PlayerManager>();
    }

    public void AttachHandToObject()
    {

        arms[0].HandPosition.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        arms[1].HandPosition.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        MoveArmsToBringPosition();

        RaycastHit hit;
        LayerMask layerMask = ~(1 << LayerMask.NameToLayer("Player"));
        Vector3 dirVector = (manager.GrabbedObject.transform.position - arms[0].HandPosition.position).normalized;
        if (Physics.Raycast(arms[0].HandPosition.position, dirVector, out hit, Mathf.Infinity, layerMask))
        {
            arms[0].HandPosition.position = hit.point;
            Debug.DrawRay(arms[0].HandPosition.position, dirVector, Color.yellow);
            //  Debug.Break();
        }
        dirVector = (manager.GrabbedObject.transform.position - arms[1].HandPosition.position).normalized;
        if (Physics.Raycast(arms[1].HandPosition.position, dirVector, out hit, Mathf.Infinity, layerMask))
        {
            arms[1].HandPosition.position = hit.point;
        }
        arms[0].HandPosition.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        arms[1].HandPosition.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }

    /// <summary>
    /// WIP 
    /// Move the arms to a position in front of the player to help positioning correctly when bring object
    /// </summary>
    private void MoveArmsToBringPosition()
    {
        arms[0].HandPosition.position = new Vector3(armsPositionForBring.position.x-0.6f, armsPositionForBring.position.y, armsPositionForBring.position.z);
        arms[1].HandPosition.position = new Vector3(armsPositionForBring.position.x + 0.6f, armsPositionForBring.position.y, armsPositionForBring.position.z);
    }

    public void DetachHand()
    {
        arms[0].HandPosition.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        arms[1].HandPosition.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }
}
