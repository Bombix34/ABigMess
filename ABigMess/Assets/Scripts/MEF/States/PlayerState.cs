using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : State
{
    protected PlayerManager manager;

    public PlayerState(ObjectManager curObject) : base(curObject)
    {
        this.curObject = curObject;
        manager = (PlayerManager)curObject;
    }

    //STATE GESTION___________________________________________________________________________________

    public override void Enter()
    {
        throw new System.NotImplementedException();
    }

    public override void Execute()
    {
        throw new System.NotImplementedException();
    }

    public override void FixedExecute()
    {
        throw new System.NotImplementedException();
    }

    public override void Exit()
    {
        throw new System.NotImplementedException();
    }
}
