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
    public Transform Visual { get; private set; }

    private Vector2 offset;
    public Vector2 Offset => offset;
    public TeamType team;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        UnitManager unitManager = UnitManager.Instance;
        TeamManager teamManager = TeamManager.Instance;

        if(OwnerClientId == GameManager.Instance.HostID.Value)
        {
            gameObject.tag = unitManager.BlueUnitTag;
            gameObject.layer = (int)Mathf.Log(teamManager.BlueLayer.value, 2);
            team = TeamType.Blue;
            //transform.Find("MinimapPoint").GetComponentInChildren<SpriteRenderer>().color = Color.green;
        }
        else
        {
            gameObject.tag = unitManager.RedUnitTag;
            gameObject.layer = (int)Mathf.Log(teamManager.RedLayer.value, 2);
            team = TeamType.Red;
            //transform.Find("MinimapPoint").GetComponentInChildren<SpriteRenderer>().color = Color.red;
        }

        gameObject.name = gameObject.name.Replace("(Clone)", $"_{gameObject.tag}");

        Visual = transform.Find("Visual");
        Stat = GetComponent<UnitStat>();
        Movement = GetComponent<UnitMovement>();
        Health = GetComponent<UnitHealth>();
        Attack = GetComponent<UnitAttack>();
        Anim = Visual.GetComponent<UnitAnimation>();

        Stat.InitCompo(this);
        Movement.InitCompo(this);
        Health.InitCompo(this);
        Attack.InitCompo(this);
        Anim.InitCompo(this);

        if (IsFriendly())
        {
            transform.Find("UnitSightMask").gameObject.SetActive(true);
            //MinimapManager.Instance.RegistViewObject(GetComponent<ViewObject>());
        }

        //GetComponent<SightObject>().Init(team);

        InitState();
    }

    public bool IsFriendly()
    {
        if(IsServer)
            return gameObject.tag == UnitManager.Instance.BlueUnitTag;
        else
            return gameObject.tag == UnitManager.Instance.RedUnitTag;
    }

    private void Update()
    {
        if (!IsSpawned)
            return;

        states[CurrentState].UpdateState();
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
