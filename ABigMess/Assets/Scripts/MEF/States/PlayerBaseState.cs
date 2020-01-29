﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseState : PlayerState
{

    public PlayerBaseState(ObjectManager curObject) : base(curObject)
    {
        stateName = "PLAYER_BASE_STATE";
        this.curObject = curObject;
        manager = (PlayerManager)curObject;
    }

    #region STATE_GESTION

    public override void Enter()
    {
    }

    public override void Execute()
    {
        manager.TryBringObject();
        manager.TryInteraction();
    }

    public override void FixedExecute()
    {
        manager.UpdateMovement();
    }

    public override void Exit()
    {
    }

    #endregion
}
