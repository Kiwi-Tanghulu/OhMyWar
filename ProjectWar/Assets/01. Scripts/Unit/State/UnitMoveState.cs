using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMoveState : UnitState
{
    protected override void OnServerEnter()
    {
        base.OnServerEnter();

        controller.Anim.SetBoolPropretyClientRpc("Move", true);
    }

    protected override void OnServerUpdate()
    {
        base.OnServerUpdate();

        controller.Movement.Move();
    }

    protected override void OnServerExit()
    {
        base.OnServerExit();

        controller.Movement.Stop();
        controller.Anim.SetBoolPropretyClientRpc("Move", false);
    }
}
