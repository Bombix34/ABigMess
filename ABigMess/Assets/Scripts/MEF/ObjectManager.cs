using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectManager : MonoBehaviour {

	protected State currentState;

	public virtual void ChangeState(State newState)
	{
		//newState==null permet de traiter les Idles des différentes config' : IA ou joueur
		if(newState==null)
			return;
        if(currentState!=null)
            currentState.Exit();
		currentState=newState;
		newState.Enter();
	}
	
	//public abstract void ReceiveMessage(Telegram message);

	public State GetCurrentState()
    {
		return currentState;
	}

}
