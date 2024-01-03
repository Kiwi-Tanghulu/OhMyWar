using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDieState : UnitState
{
    protected override void OnServerEnter()
    {
        base.OnServerEnter();

        Debug.Log("die");
        controller.Anim.SetTriggerPropretyClientRpc("Die");
    }
}
