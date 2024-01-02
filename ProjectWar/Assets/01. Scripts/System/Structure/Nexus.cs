using Unity.Netcode;
using UnityEngine;

public class Nexus : StructureBase, IUnitSpawner
{
    [field:SerializeField] public Transform SpawnPosition {get; private set;} = null;
    [SerializeField] NetworkPrefabsList unitPrefabs = null;

    [Space(10f)]
    [SerializeField] Sprite blueSprite = null;
    [SerializeField] Sprite redSprite = null;

    private ulong ownerID = ulong.MaxValue;
    public ulong OwnerID => ownerID;

    private ulong attackerID = ulong.MaxValue;

    public bool IsEmpty => (ownerID == ulong.MaxValue);

    public void Init(ulong ownerID)
    {
        this.ownerID = ownerID;
    }
	
    public void SpawnUnit(int unitIndex, int lineIndex)
    {
        UnitManager.Instance.SpawnUnit((UnitType)unitIndex, NetworkManager.LocalClientId,
            SpawnPosition.position, SpawnPosition.position);
    }

    public override void OnDamaged(int damage = 0, NetworkObject performer = null, Vector3 point = default)
    {
        if(IsEmpty) // 비어있을 때
        {
            if(attackerID == ulong.MaxValue) // 클린한 상태
            {
                attackerID = performer.OwnerClientId;
            }
        }
        

        
        // base.OnDamaged(damage, performer, point);
    }

    public void ChangeOwner(NetworkClient networkCliet)
    {
        this.ownerID = networkCliet.ClientId;
        ModifyHP(maxHP);

        // change spawn position
    }
}
