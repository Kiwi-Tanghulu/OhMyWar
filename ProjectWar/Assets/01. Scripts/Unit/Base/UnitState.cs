using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class UnitState : NetworkBehaviour
{
    protected UnitController controller;
    public UnitStateType stateType;

    public void InitState(UnitController _controller, UnitStateType _type)
    {
        controller = _controller;
        stateType = _type;

        if (IsServer)
            OnServerInit();

        ClientInit();
    }
    protected virtual void OnServerInit()
    {
        if (!IsServer)
            return;
    }
    protected virtual void ClientInit()
    {

    }

    public void EnterState()
    {
        if (IsServer)
            OnServerEnter();

        ClientEnter();
    }
    protected virtual void OnServerEnter()
    {
        if (!IsServer)
            return;
    }
    protected virtual void ClientEnter()
    {

    }

    public void UpdateState()
    {
        if (IsServer)
            OnServerUpdate();

        ClientUpdate();
    }
    protected virtual void OnServerUpdate()
    {
        if (!IsServer)
            return;
    }
    protected virtual void ClientUpdate()
    {
        
    }

    public void ExitState()
    {
        if (IsServer)
            OnServerExit();

        ClientExit();
    }
    protected virtual void OnServerExit()
    {
        if (!IsServer)
            return;
    }
    protected virtual void ClientExit()
    {

    }
}
