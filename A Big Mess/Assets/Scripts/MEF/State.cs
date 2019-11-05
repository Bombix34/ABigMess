using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State  
{

	public string stateName;

	protected ObjectManager curObject;

	public State(ObjectManager curObject)
    {
		this.curObject=curObject;
	}
	
	public abstract void Enter();
	public abstract void Execute();
	public abstract void Exit();
    //public abstract void OnMessage(Telegram message);
}
