using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(UnitMovement))]
[RequireComponent(typeof(UnitHealth))]
[RequireComponent(typeof(UnitStat))]
public class UnitController : NetworkBehaviour
{
    [SerializeField] private UnitInfoSO info;
    public UnitInfoSO Info => info;
    private Dictionary<UnitStateType, UnitState> states;
    [field: SerializeField]
    public UnitStateType CurrentState { get; private set; }

    public UnitMovement Movement { get; private set; }
    public UnitHealth Health { get; private set; }
    public UnitAttack Attack { get; private set; }
    public UnitAnimation Anim { get; private set; }
    public UnitStat Stat { get; private set; }

    private void Awake()
    {
        Movement = GetComponent<UnitMovement>();
        Health = GetComponent<UnitHealth>();
        Attack = GetComponent<UnitAttack>();
        Stat = GetComponent<UnitStat>();
        Anim = transform.Find("Visual").GetComponent<UnitAnimation>();

        Movement.InitCompo(this);
        Health.InitCompo(this);
        Attack.InitCompo(this);
        Anim.InitCompo(this);
        Stat.InitCompo(this);

        InitState();
    }

    private void Update()
    {
        states[CurrentState].UpdateState();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner)
            gameObject.layer = 6;
        else 
            gameObject.layer = 7;

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
