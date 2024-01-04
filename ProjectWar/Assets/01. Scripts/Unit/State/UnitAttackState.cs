using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class UnitAttackState : UnitState
{
    private bool isAttack = false;

    protected override void OnServerEnter()
    {
        base.OnServerEnter();

        controller.Anim.AnimStartEvent += StartAttack;
        controller.Anim.OnAnimEvent += controller.Attack.Attack;
        controller.Anim.AnimEndEvent += EndAttack;
    }

    protected override void OnServerUpdate()
    {
        base.OnServerUpdate();

        if (controller.Attack.StartAttack())
            controller.Anim.SetTriggerPropretyClientRpc("Attack");

        if(!isAttack)
            IdleHandle();
    }

    protected override void OnServerExit()
    {
        base.OnServerExit();

        controller.Anim.AnimStartEvent -= StartAttack;
        controller.Anim.OnAnimEvent -= controller.Attack.Attack;
        controller.Anim.AnimEndEvent -= EndAttack;
    }

    private void IdleHandle()
    {
        if (!controller.Attack.ShouldAttack)
            controller.ChangeState(UnitStateType.Idle);
    }

    private void StartAttack()
    {
        isAttack = true;
        controller.Attack.OnAttackStartEvetn?.Invoke();
    }

    private void EndAttack()
    {
        isAttack = false;
    }
}
