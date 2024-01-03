using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStunState : UnitState
{
    [SerializeField] private float stunTime;

    protected override void OnServerEnter()
    {
        base.OnServerEnter();

        controller.Anim.SetTriggerPropretyClientRpc("Stun");
        controller.Anim.AnimStartEvent += StartStun;
        StartCoroutine(StunCo());
    }

    protected override void ClientExit()
    {
        base.ClientExit();

        controller.Anim.AnimStartEvent -= StartStun;
    }

    private IEnumerator StunCo()
    {
        yield return new WaitForSeconds(stunTime);

        controller.ChangeState(UnitStateType.Idle);

        controller.Anim.SetBoolPropretyClientRpc("IsStun", false);
    }

    private void StartStun()
    {
        controller.Anim.SetBoolPropretyClientRpc("IsStun", true);
    }
}
