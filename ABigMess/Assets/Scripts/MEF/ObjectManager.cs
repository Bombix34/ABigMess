using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectManager : MonoBehaviour {

	protected State currentState;

	public virtual void ChangeState(State newState)
	{
        if(currentState!=null)
        {
            currentState.Exit();
        }
		currentState=newState;
		currentState.Enter();
	}
	
	//public abstract void ReceiveMessage(Telegram message);

	public State GetCurrentState()
    {
		return currentState;
	}

}
