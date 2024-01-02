using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Playables;

public class UnitController : NetworkBehaviour
{
    private Dictionary<UnitStateType, UnitState> states;
    [field: SerializeField]
    public UnitStateType CurrentState { get; private set; }

    private void Awake()
    {
        InitState();
    }

    private void Update()
    {
        states[CurrentState].UpdateState();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        ChangeState(UnitStateType.Idle);
    }

    #region ChangeState
    public void ChangeState(UnitStateType type)
    {
        if (!IsServer)
            return;

        ChangeStateServerRpc(type);
    }
    [ServerRpc]
    private void ChangeStateServerRpc(UnitStateType type)
    {
        ChangeStateClientRpc(type);
    }
    [ClientRpc]
    private void ChangeStateClientRpc(UnitStateType type)
    {
        if (states.TryGetValue(type, out UnitState state))
        {
            if (state == null)
            {
                Debug.LogError($"Unit{type}State없음");
                return;
            }
        }

        states[CurrentState]?.ExitState();
        CurrentState = type;
        states[CurrentState].EnterState();
    }
    #endregion

    private void InitState()
    {
        states = new();
        Transform stateContainer = transform.Find("StateContainer");

        foreach (UnitStateType type in Enum.GetValues(typeof(UnitStateType)))
        {
            string stateName = $"Unit{type}State";
            Transform stateTrm = stateContainer.Find(stateName);
            UnitState state = (UnitState)stateTrm?.GetComponent(stateName);
            
            if (state == null || stateTrm == null)
            {
                Debug.LogError($"Unit{type}State없음");
                continue;
            }

            state.InitState(this, type);
            states.Add(type, state);
        }
    }
}
