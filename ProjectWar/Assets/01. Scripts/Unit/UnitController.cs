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

    private Vector2 offset;
    public Vector2 Offset => offset;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        UnitManager unitManager = UnitManager.Instance;
        TeamManager teamManager = TeamManager.Instance;

        if(IsServer && IsOwner)
        {
            gameObject.tag = unitManager.BlueUnitTag;
            gameObject.layer = (int)Mathf.Log(teamManager.BlueLayer.value, 2);
        }
        else
        {
            gameObject.tag = unitManager.RedUnitTag;
            gameObject.layer = (int)Mathf.Log(teamManager.RedLayer.value, 2);
        }

        gameObject.name = gameObject.name.Replace("(Clone)", $"_{gameObject.tag}");

        Stat = GetComponent<UnitStat>();
        Movement = GetComponent<UnitMovement>();
        Health = GetComponent<UnitHealth>();
        Attack = GetComponent<UnitAttack>();
        Anim = transform.Find("Visual").GetComponent<UnitAnimation>();

        Stat.InitCompo(this);
        Movement.InitCompo(this);
        Health.InitCompo(this);
        Attack.InitCompo(this);
        Anim.InitCompo(this);

        InitState();
    }

    private void Update()
    {
        if (!IsSpawned)
            return;

        states[CurrentState].UpdateState();

        if (Input.GetKeyUp(KeyCode.Q))
            Health.Stun();
    }

    public void SetOffset(Vector2 offset)
    {
        this.offset = offset;
    }

    #region ChangeState
    public void ChangeState(UnitStateType type)
    {
        if (!IsServer)
            return;

        ChangeStateServerRpc(type);
    }
    [ServerRpc(RequireOwnership = false)]
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
                Debug.LogError($"Unit{type}State");
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
            Debug.Log(stateName);
            Transform stateTrm = stateContainer.Find(stateName);
            UnitState state = (UnitState)stateTrm?.GetComponent(stateName);
            
            if (state == null || stateTrm == null)
            {
                Debug.LogError($"Unit{type}State����");
                continue;
            }

            state.InitState(this, type);
            states.Add(type, state);
        }

        ChangeState(UnitStateType.Idle);
    }
}
