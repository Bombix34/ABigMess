using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementAuto : MonoBehaviour
{
    public List<Transform> destinations;
    public int curIndex = 0;

    public PlayerManager playerToMove;

    bool isMoving = false;

    void Start()
    {
        playerToMove.Movement.CanMove = false;
    }

    private void Update()
    {
        TryInput();
    }

    public void TryInput()
    {
        if(isMoving || curIndex>=destinations.Count)
        {
            return;
        }
        if(playerToMove.Inputs.GetGrabInputDown())
        {
            StartCoroutine(MovePlayer());
        }
    }

    private IEnumerator MovePlayer()
    {
        isMoving = true;
        playerToMove.Movement.CanMove =true;
        Vector3 dirVector = (destinations[curIndex].position - playerToMove.transform.position).normalized;
        float amplitude = (destinations[curIndex].position - playerToMove.transform.position).magnitude;
        while(amplitude>1.1f)
        {
            playerToMove.Movement.DoMove(dirVector*1.6f);
            playerToMove.Renderer.UpdateAnimation((dirVector * 1.6f).magnitude);
            amplitude = (destinations[curIndex].position - playerToMove.transform.position).magnitude;
            print(amplitude);
            yield return new WaitForSeconds(0.01f);
        }
        playerToMove.Movement.CanMove = false;
        curIndex++;
        isMoving = false;
        if(curIndex==1)
        {
            StartCoroutine(MovePlayer());
        }
    }
}
