using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class UnitAnimation : UnitComponent
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    [ClientRpc]
    public void SetBoolPropretyClientRpc(string name, bool value)
    {
        anim.SetBool(name, value);
    }

    [ClientRpc]
    public void SetTriggerPropretyClientRpc(string name)
    {
        anim.SetTrigger(name);
    }
}
