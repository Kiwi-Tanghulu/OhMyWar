using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitIdleState : UnitState
{
    protected override void OnServerEnter()
    {
        base.OnServerEnter();

        //controller.Movement.Stop();
        controller.Anim.SetBoolPropretyClientRpc("Move", false);
    }

    protected override void OnServerUpdate()
    {
        base.OnServerUpdate();

        MoveHandle();
        AttackHandle();
    }

    private void MoveHandle()
    {
        if (controller.Movement.ShouldMove)
            controller.ChangeState(UnitStateType.Move);
    }

    private void AttackHandle()
    {
        if (controller.Attack.ShouldAttack)
            controller.ChangeState(UnitStateType.Attack);
    }
}
