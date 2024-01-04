using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : NetworkBehaviour
{
    private int gold = 0;
    public int Gold => gold;
    public int TotalGold = 0;
    public int MaxGold { get; private set; } = 500;

    public bool IsBlue = false;

    private List<PlayerComponent> components;

    public TeamType team { get; private set; }

    private GameObject sightMask = null;

    public List<StatData> Buffs = new List<StatData>();

    public override void OnNetworkSpawn()
    {
        IsBlue = (OwnerClientId == GameManager.Instance.HostID.Value);
        IngameManager.Instance.RegisterPlayer(this);

        if(IsOwner)
            IngameManager.Instance.ToggleCurrentSpawner(this, 1);

        components = new List<PlayerComponent>();
        GetComponents<PlayerComponent>(components);
        components.ForEach(component => component?.Init(this));

        GetComponent<PlayerSkillHandler>().Init();

        if (IsOwner)
            IngameManager.Instance.OwnerPlayer = this;

        if(IsServer && IsOwner)
        {
            gameObject.layer = (int)Mathf.Log(TeamManager.Instance.BlueLayer, 2);
            team = TeamType.Blue;
        }
        else
        {
            gameObject.layer = (int)Mathf.Log(TeamManager.Instance.RedLayer, 2);
            team = TeamType.Red;
        }

        sightMask = transform.Find("PlayerSightMask").gameObject;
        sightMask.SetActive(IsOwner);
    }

    private void Update()
    {
        if(IsOwner == false)
            return;

        if (Keyboard.current.qKey.isPressed)
            ModifyGold(1000);
    }

    public override void OnNetworkDespawn()
    {
        components.ForEach(component => component?.Release());
    }

    public void ModifyGold(int value)
    {
        int prevGold = gold;

        gold += value;
        gold = Mathf.Clamp(gold, 0, MaxGold);
        TotalGold += Mathf.Max(0, gold - prevGold);
    }

    public void SetMaxGold(int value)
    {
        MaxGold = value;
    }
}
