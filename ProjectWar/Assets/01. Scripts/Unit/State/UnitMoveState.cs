using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMoveState : UnitState
{
    private GameObject walkParticle = null;

    protected override void ClientInit()
    {
        base.ClientInit();
        walkParticle = controller.transform.Find("Visual/Shadow/WalkParticle").gameObject;
    }

    protected override void OnServerEnter()
    {
        base.OnServerEnter();

        controller.Anim.SetBoolPropretyClientRpc("Move", true);
        walkParticle.SetActive(true);
    }

    protected override void OnServerUpdate()
    {
        base.OnServerUpdate();

        controller.Movement.Move();

        IdleHandle();
        AttackHandle();
    }

    protected override void OnServerExit()
    {
        base.OnServerExit();

        controller.Anim.SetBoolPropretyClientRpc("Move", false);
        walkParticle.SetActive(false);
    }

    private void IdleHandle()
    {
        if (controller.Movement.IsArrived)
        {
            controller.ChangeState(UnitStateType.Idle);
        }
    }

    private void AttackHandle()
    {
        if (controller.Attack.ShouldAttack)
        {
            controller.ChangeState(UnitStateType.Attack);
        }
    }
}
