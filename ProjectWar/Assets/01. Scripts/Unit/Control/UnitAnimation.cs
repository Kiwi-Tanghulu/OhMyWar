using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class UnitAnimation : UnitComponent
{
    private Animator anim;

    public event Action AnimStartEvent;
    public event Action OnAnimEvent;
    public event Action AnimEndEvent;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void SetBoolProprety(string name, bool value)
    {
        anim.SetBool(name, value);
    }

    public void SetTriggerProprety(string name)
    {
        anim.SetTrigger(name);
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

    public void InvokeAnimStartEvent() => AnimStartEvent?.Invoke(); 
    public void InvokeOnAnimEvent() => OnAnimEvent?.Invoke(); 
    public void InvokeAnimEndEvent() => AnimEndEvent?.Invoke(); 
}
