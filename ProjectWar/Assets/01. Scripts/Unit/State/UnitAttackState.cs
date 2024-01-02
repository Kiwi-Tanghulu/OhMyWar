using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class UnitAttackState : UnitState
{
    protected override void OnServerEnter()
    {
        base.OnServerEnter();

        controller.Anim.OnAnimEvent += controller.Attack.Attack;
    }

    protected override void OnServerUpdate()
    {
        base.OnServerUpdate();

        if (controller.Attack.StartAttack())
            controller.Anim.SetTriggerPropretyClientRpc("Attack");
        IdleHandle();
    }

    protected override void OnServerExit()
    {
        base.OnServerExit();

        controller.Anim.OnAnimEvent -= controller.Attack.Attack;
    }

    private void IdleHandle()
    {
        if (!controller.Attack.ShouldAttack)
            controller.ChangeState(UnitStateType.Idle);
    }
}
