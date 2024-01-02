using Unity.Netcode;
using UnityEngine;

public class Nexus : StructureBase, IUnitSpawner
{
    [SerializeField] Transform spawnPosition = null;
    [SerializeField] NetworkPrefabsList unitPrefabs = null;

    [Space(10f)]
    [SerializeField] Sprite blueSprite = null;
    [SerializeField] Sprite redSprite = null;

    private ulong ownerID = 0;
    public ulong OwnerID => ownerID;

    public void Init(ulong ownerID)
    {
        this.ownerID = ownerID;
    }
	
    public void SpawnUnit(int unitIndex, int lineIndex)
    {
        UnitManager.Instance.SpawnUnit((UnitType)unitIndex, NetworkManager.LocalClientId,
            spawnPosition.position, spawnPosition.position);
    }

    public void ChangeOwner(NetworkClient networkCliet)
    {
        this.ownerID = networkCliet.ClientId;
        ModifyHP(maxHP);

        // change spawn position
    }
}
