using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDieState : UnitState
{
    protected override void OnServerEnter()
    {
        base.OnServerEnter();
        
        //controller.Anim.SetTriggerPropretyClientRpc("Die");
    }

    protected override void ClientEnter()
    {
        base.ClientEnter();
        controller.Visual.gameObject.SetActive(false);
        UnitManager.Instance.DespawnUnit(controller);
    }
}
