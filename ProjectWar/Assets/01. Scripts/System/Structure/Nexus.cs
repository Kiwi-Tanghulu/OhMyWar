using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class StatData
{
    public UnitStatType type;
    public int value;
}

public class Nexus : StructureBase, IUnitSpawner
{
    [field:SerializeField] public Transform SpawnPosition {get; private set;} = null;
    [SerializeField] NetworkPrefabsList unitPrefabs = null;

    [Space(10f)]
    [SerializeField] Sprite blueSprite = null;
    [SerializeField] Sprite redSprite = null;
    //[SerializeField] private MinimapImage minimapImage;

    [Space(10f)]
    public List<StatData> Buffs = new List<StatData>();

    [SerializeField] private int line;

    private SpriteRenderer spRenderer = null;
    private GameObject sightMask = null;

    private ulong ownerID = ulong.MaxValue;
    public ulong OwnerID => ownerID;

    private ulong attackerID = ulong.MaxValue;

    public bool IsEmpty => (ownerID == ulong.MaxValue);

    protected override void Awake()
    {
        base.Awake();
        spRenderer = GetComponent<SpriteRenderer>();
        sightMask = transform.Find("NexusSightMask").gameObject;
    }
	
    public void SpawnUnit(int unitIndex, int lineIndex)
    {
        UnitManager.Instance.SpawnUnit((UnitType)unitIndex, NetworkManager.LocalClientId,
            line, 1);
    }

    public override void OnDamaged(int damage = 0, NetworkObject performer = null, Vector3 point = default)
    {
        if (isDestroyed)
            return;

        if(performer.OwnerClientId == NetworkManager.Singleton.LocalClientId)
        {
            sightMask.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(DelayCoroutine(4f, () => {sightMask.SetActive(false);Debug.Log("invoked");}));
        }

        if (IsEmpty) // 비어있을 때
        {
            if(attackerID == ulong.MaxValue) // 클린한 상태
                attackerID = performer.OwnerClientId;
            else if(attackerID != performer.OwnerClientId) // 다른 애가 때린 거라면
            {
                ModifyHP(damage); // 힐 하기

                if(HP >= maxHP) // 공격자 전환
                    attackerID = performer.OwnerClientId;
            }
            else
            {
                ModifyHP(-damage);

                if(HP <= 0)
                {
                    isDestroyed = true;
                    OnDestroyedEvent?.Invoke(performer);
                    
                    OnDie(performer);
                }
            }
        }
        else
        {

            base.OnDamaged(damage, performer, point);        
        }
    }

    public override void OnDie(NetworkObject performer)
    {
        StopAllCoroutines();

        ChangeOwner(performer);
        //IngameManager.Instance.ToggleCurrentSpawner(IngameManager.Instance.OwnerPlayer, line);
        base.OnDie(performer);
    }

    [ClientRpc]
    private void ChangerTeamClientRpc(int value)
    {
        gameObject.layer = value;
        sightMask.SetActive(false);

        if (1 << (gameObject.layer) == TeamManager.Instance.BlueLayer)
        {
            IngameManager.Instance.BluePlayer.Buffs.AddRange(Buffs);
            spRenderer.sprite = blueSprite;
            //minimapImage.ChangeImage(TeamType.Blue);
            if (IsHost)
            {
                StopAllCoroutines();
                sightMask.SetActive(IsHost);
            }
        }
        else
        {
            IngameManager.Instance.RedPlayer.Buffs.AddRange(Buffs);
            spRenderer.sprite = redSprite;
            //minimapImage.ChangeImage(TeamType.Red);
            if (!IsHost)
            {
                StopAllCoroutines();
               sightMask.SetActive(!IsHost);
            }
        }
    }

    public void ChangeOwner(NetworkObject performer)
    {
        this.ownerID = performer.OwnerClientId;
        ModifyHP(maxHP);

        isDestroyed = false;

        if(ownerID == IngameManager.Instance.BluePlayer.OwnerClientId)
            spRenderer.sprite = blueSprite;
        else
            spRenderer.sprite = redSprite;
        // change spawn position

        ChangerTeamClientRpc(performer.gameObject.layer);
    }
}
