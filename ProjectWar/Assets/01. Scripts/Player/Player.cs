using System;
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
    public NetworkVariable<CharacterType> characterType = new NetworkVariable<CharacterType>();

    private GameObject sightMask = null;

    public List<StatData> Buffs = new List<StatData>();

    public event Action<int, int> OnGoldChanged;

    public void SetCharacterType(CharacterType type)
    {
        characterType.Value = type;
        characterType.SetDirty(true);
    }

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
        {
            Debug.Log(characterType);
            GameInfoPanel gameInfoPanel = FindObjectOfType<GameInfoPanel>();
            gameInfoPanel.SettingSkillIcon(characterType.Value);
            IngameManager.Instance.OwnerPlayer = this;
        }

        if (OwnerClientId == GameManager.Instance.HostID.Value)
        {
            gameObject.layer = (int)Mathf.Log(TeamManager.Instance.BlueLayer, 2);
            team = TeamType.Blue;
        }
        else
        {
            gameObject.layer = (int)Mathf.Log(TeamManager.Instance.RedLayer, 2);
            team = TeamType.Red;
        }

        GetComponent<SightObject>().Init(team);
        sightMask = transform.Find("PlayerSightMask").gameObject;
        sightMask.SetActive(IsOwner);
        
        if (TeamManager.Instance.IsFriendly(gameObject))
        {
            MinimapManager.Instance.RegistViewObject(GetComponent<ViewObject>());
        }
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

        OnGoldChanged?.Invoke(gold, MaxGold);
    }

    public void SetMaxGold(int value)
    {
        MaxGold = value;
    }
}
