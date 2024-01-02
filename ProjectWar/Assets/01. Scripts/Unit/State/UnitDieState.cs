using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDieState : UnitState
{
    protected override void OnServerEnter()
    {
        base.OnServerEnter();

        controller.Anim.SetTriggerPropretyClientRpc("Die");
    }
}
