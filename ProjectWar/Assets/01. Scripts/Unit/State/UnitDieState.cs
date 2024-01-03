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
        Instantiate(controller.Health.deadEffect, transform.position, Quaternion.identity);
        Debug.Log("die");
    }
}
