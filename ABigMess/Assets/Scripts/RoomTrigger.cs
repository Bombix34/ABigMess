using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    [SerializeField]
    int roomNb = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerManager>().CurrentRoomNb = roomNb;
            CameraManager.Instance.SwitchCamera(roomNb);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerManager>().CurrentRoomNb = roomNb;
            CameraManager.Instance.SwitchCamera(roomNb);
        }
    }
}

