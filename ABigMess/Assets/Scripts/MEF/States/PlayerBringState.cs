using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBringState : PlayerState
{
    InteractObject objectBring;

    public PlayerBringState(ObjectManager curObject) : base(curObject)
    {
        this.curObject = curObject;
        manager = (PlayerManager)curObject;
    }

    public PlayerBringState(ObjectManager curObject, InteractObject obj) : base(curObject)
    {
        this.curObject = curObject;
        manager = (PlayerManager)curObject;
        objectBring = obj;
    }

    #region STATE_GESTION

    public override void Enter()
    {
        GameManager.Instance.TasksManager.UpdateTasksState();
    }

    public override void Execute()
    {
        manager.DropBringObject();
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