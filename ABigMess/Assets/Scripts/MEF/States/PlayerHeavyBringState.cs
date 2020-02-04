using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeavyBringState : PlayerState
{
    InteractObject objectBring;

    public PlayerHeavyBringState(ObjectManager curObject) : base(curObject)
    {
        this.curObject = curObject;
        manager = (PlayerManager)curObject;
        this.stateName = "PLAYER_HEAVY_BRING_OBJECT";
    }

    public PlayerHeavyBringState(ObjectManager curObject, InteractObject obj) : base(curObject)
    {
        this.curObject = curObject;
        manager = (PlayerManager)curObject;
        objectBring = obj;
        this.stateName = "PLAYER_HEAVY_BRING_OBJECT";
    }

    #region STATE_GESTION

    public override void Enter()
    {
        manager.Movement.ResetVelocity();
    }

    public override void Execute()
    {
        manager.DropBringObject();
    }

    public override void FixedExecute()
    {
    }

    public override void Exit()
    {
    }

    #endregion
}