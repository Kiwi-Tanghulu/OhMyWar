using Unity.Netcode;
using UnityEngine;

public class Nexus : StructureBase, IUnitSpawner
{
    [field:SerializeField] public Transform SpawnPosition {get; private set;} = null;
    [SerializeField] NetworkPrefabsList unitPrefabs = null;

    [Space(10f)]
    [SerializeField] Sprite blueSprite = null;
    [SerializeField] Sprite redSprite = null;

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
            SpawnPosition.position, SpawnPosition.position);
    }

    public override void OnDamaged(int damage = 0, NetworkObject performer = null, Vector3 point = default)
    {
        if (isDestroyed)
            return;

        if (IsEmpty) // 비어있을 때
        {
            if(attackerID == ulong.MaxValue) // 클린한 상태
                attackerID = performer.OwnerClientId;
            else if(attackerID != performer.OwnerClientId) // 다른 애가 때린 거라면
            {
                ModifyHP(damage); // 힐 하기
                OnDamagedEvent?.Invoke(performer, point, damage);

                if(HP >= maxHP) // 공격자 전환
                    attackerID = performer.OwnerClientId;
            }
            else
            {
                ModifyHP(-damage);
                OnDamagedEvent?.Invoke(performer, point, damage);

                if(HP <= 0)
                {
                    isDestroyed = true;
                    OnDestroyedEvent?.Invoke(performer);
                    TeamManager.Instance.ChangeTeam(gameObject, performer.gameObject);
                    OnDie(performer);
                }
            }
        }
        else
            base.OnDamaged(damage, performer, point);        
    }

    public override void OnDie(NetworkObject performer)
    {
        ChangeOwner(performer);
        base.OnDie(performer);
    }

    [ClientRpc]
    private void ChangerTeamClientRpc(int value)
    {
        gameObject.layer = value;
        sightMask.SetActive(false);

        if(1 << (gameObject.layer) == TeamManager.Instance.BlueLayer)
        {
            spRenderer.sprite = blueSprite;
            sightMask.SetActive(IsHost);
        }
        else
        {
            spRenderer.sprite = redSprite;
            sightMask.SetActive(!IsHost);
        }

        Debug.Log("chagne owner");
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
